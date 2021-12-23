using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace DermaScan.Functions
{
    public class Dermatoloog
    {
        public static void Scraper(int days, int time ,string email)
        {
            // Configure Chrome options to use in chrome driver
            ChromeOptions option = new ChromeOptions();
            // option.AddArgument("--headless");
            // option.AddArgument("--silent");
            // option.AddArgument("--disable-gpu");
            option.AddArgument("--log-level=3");


            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.SuppressInitialDiagnosticInformation = true;

            // Make new Chrome driver and go to Youtube
            IWebDriver driver = new ChromeDriver(service, option);
            driver.Navigate().GoToUrl("https://www.dermatologieaarschot.be/");

            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.XPath("/html/body/header/div[2]/div/div[3]/ul/li[1]/a")).Click();

            System.Threading.Thread.Sleep(500);

            driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/div[1]")).Click();

            System.Threading.Thread.Sleep(500);

            // Test page
            /*driver.FindElement(By.XPath(
                    "/html/body/div[1]/div[4]/div[1]/div/div/div[2]/div[2]/div[1]/div[2]/table/tbody/tr/td/div/div[2]/a"))
                .Click();*/

            System.Threading.Thread.Sleep(1000);

            var dokter = driver.FindElements(By.XPath(
                "/html/body/div/div[4]/div[1]/div/div/div[2]/div[2]/div/div[2]/table/tbody/tr"));
            

            var appointment = false;
            while (appointment == false)
            {
                for (var j = 0; j < days; j++)
                {
                    for (var k = 1; k < dokter.Count + 1; k++)
                    {
                        var date = driver.FindElement(
                            By.XPath("/html/body/div/div[4]/div[1]/div/div/div[2]/table/tbody/tr/td[3]/h2"));

                        var afspraak = driver.FindElements(By.XPath(
                            "/html/body/div/div[4]/div[1]/div/div/div[2]/div[2]/div[" + k + "]/div[2]/table/tbody/tr"));

                        foreach (var i in afspraak)
                        {
                            int tr = 1;
                            if (i.Text.Contains("afspraak dr Kerre - Aarschot") || i.Text.Contains("VRIJ PATIENT"))
                            {
                                var url = i.FindElement(By.XPath(
                                    "/html/body/div/div[4]/div[1]/div/div/div[2]/div[2]/div[" + k + "]/div[2]/table/tbody/tr[" + tr +"]/td[2]/a"));

                                String payLoad = date.Text + " " + i.Text;
                                appointment = true;
                                SendMail.Appointment(payLoad, url.GetAttribute("href"), email);
                                Console.WriteLine("Appointment was found: " + payLoad);
                                Console.WriteLine(url.GetAttribute("href"));
                                break;
                            }

                            tr++;
                        }

                        if (appointment == true)
                        {
                            break;
                        }

                        System.Threading.Thread.Sleep(500);
                    }

                    if (appointment == true)
                    {
                        break;
                    }

                    driver.FindElement(
                            By.XPath("/html/body/div[1]/div[4]/div[1]/div/div/div[2]/table/tbody/tr/td[4]/a/i"))
                        .Click();
                    System.Threading.Thread.Sleep(500);
                }

                if (appointment == false)
                {
                    Console.WriteLine("Search was not successful.");
                    for (int i = 0; i < time; i++)
                    {
                        if (i == time - 1)
                        {
                            Console.WriteLine("Retrying in " + (time - i) + " minute.");
                            System.Threading.Thread.Sleep(60000);
                        }
                        else
                        {
                            Console.WriteLine("Retrying in " + (time - i) + " minutes.");
                            System.Threading.Thread.Sleep(60000);
                        }
                    }

                    Console.WriteLine("Retrying now.");

                    driver.Navigate().Refresh();
                    System.Threading.Thread.Sleep(2000);
                }
                else
                {
                    Console.WriteLine("Search was successful, an e-mail has been sent");
                    driver.Quit();
                }
            }
        }
    }
}