using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

string[] bannedWords = { };

// Type your username and press enter
Console.WriteLine("Would you like to exclude any words?");

// Create a string variable and get user input from the keyboard and store it in the variable
string excludeYesOrNo = Console.ReadLine().ToLower();

if (excludeYesOrNo.Equals("yes"))
{
    //Console.WriteLine("How many words would you like to exclude?");
    //string numberOfBannedWords = Console.ReadLine().ToLower();

    Console.WriteLine("Please write out the words you would like to exclude on one line seperated by spaces");
    string ExcludeTheseWords = Console.ReadLine().ToLower();

    bannedWords = ExcludeTheseWords.Split(" ");
}

IWebDriver driver = new ChromeDriver();
driver.Navigate().GoToUrl("https://en.wikipedia.org/wiki/Microsoft#History");

string text = "";

for (int i = 1; i < 71; i++)
{
    text += driver.FindElement(By.XPath($"/html/body/div[3]/div[3]/div[5]/div[1]/p[{i}]")).GetAttribute("textContent");
}

text = text.Replace(",", "").Replace(".", "").ToLower();

IDictionary<string, int> wordCounter = new Dictionary<string, int>();

string[] words = text.Split(" ");

foreach (var word in words)
{

    bool keyExists = wordCounter.ContainsKey(word);

    if (keyExists == false)
    {
        wordCounter[word] = 1;
    } else
    {
        wordCounter[word] += 1;
    }

}

if (bannedWords != null)
{
    foreach (var kvp in wordCounter)
    {
        foreach (var word in bannedWords)
            if (kvp.Key.Equals(word))
            {
                wordCounter.Remove(kvp);
            }
    }
}
var orderedWordCounter = wordCounter.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value).Skip(10).Take(10);

foreach (var kvp in orderedWordCounter)
{
    Console.WriteLine("{0} # of occurrences: {1}", kvp.Key, kvp.Value);
}
