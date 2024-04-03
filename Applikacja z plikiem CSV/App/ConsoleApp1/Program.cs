
using ConsoleApp1;
using CsvHelper;
using System.Globalization;


// MY BASIC LINQ KNOWLEDGE - example: Google apps - working with CSV file. 

// Each function (on the bottom) shows examples of how we can use LINQ when working with collectives.

//Whole directory comes with my notes in polish.


string csvpath = @"C:\Users\i5\Desktop\C#\LINQ - notatki\Applikacja z plikiem CSV/googleplaystore1.csv"; //desktop path to your csv file
var googleApps = LoadGoogleAps(csvpath);


static void Display(IEnumerable<GoogleApp> googleApps)
{
    foreach(var googleApp in googleApps)
    {
        Console.WriteLine(googleApp);
    }
}

static void DataSetOperation(IEnumerable<GoogleApp> googleApps)
{
    var paidAppsCategories = googleApps.Where(a => a.Type == ConsoleApp1.Type.Paid)
        .Select(a => a.Category).Distinct();

    Console.WriteLine($"Paid apps categories: {string.Join(", ", paidAppsCategories)}");

    // Working with sets

    var setA = googleApps.Where(a => a.Rating > 4.7 && a.Type == ConsoleApp1.Type.Paid && a.Reviews > 1000);

    var setB = googleApps.Where(a => a.Name.Contains("Pro") && a.Rating > 4.6 && a.Reviews > 10000);

    //Display(setA);

    //Console.WriteLine("******************");

    //Display(setB);

    var appUnion = setA.Union(setB); // gotta be the same type
    Console.WriteLine("Apps Union: ");
    Display(appUnion);   


    var appsIntersect = setA.Intersect(setB);
    Console.WriteLine("Apps Intersected: ");
    Display(appsIntersect);


    var appsExcept = setA.Except(setB);
    Console.WriteLine("Apps Except: ");
    Display(appsExcept);

}
static void GetData(IEnumerable<GoogleApp> googleApps)
{
    //bool IsHighRatedApp(GoogleApp app)
    //{
    //    return app.Rating > 4.6;      // Without LINQ
    //}

    var highRatedApps = googleApps.Where(app => app.Rating > 4.6); // zamiast funkcji boolionowej(IsHighratedApp) -> wyrażenie lambda / instead bool function
    var hightRatedBeautyApps = googleApps.Where(app => app.Rating > 4.6 && app.Category == Category.BEAUTY);
    Display(hightRatedBeautyApps);

    var firstHighRatedBeautyApp = hightRatedBeautyApps.Last(app => app.Reviews < 300);
}

static void ProjectData(IEnumerable<GoogleApp> googleApps)
{
    var hightRatedBeautyApps = googleApps.Where(app => app.Rating > 4.6 && app.Category == Category.BEAUTY);
    var hightRatedBeautyAppsNames = hightRatedBeautyApps.Select(app => app.Name);

    var dtos = hightRatedBeautyApps.Select(app => new GoogleAppDto()
    {
        Name = app.Name,
        Reviews = app.Reviews

    });
    
    //foreach( var dto in dtos)
    //{
    //    Console.WriteLine($"{dto.Name}: {dto.Reviews}");
    //}

    var generes = hightRatedBeautyApps.SelectMany(app => app.Genres);

    Console.WriteLine(String.Join(":", generes));



  //Console.WriteLine(String.Join(",",hightRatedBeautyAppsNames));

}

static void OrderData(IEnumerable<GoogleApp> googleApps)
{
     var hightRatedBeautyApps = googleApps.Where(app => app.Rating > 4.6 && app.Category == Category.BEAUTY);

    var SortedResault = hightRatedBeautyApps.OrderBy(app => app.Rating).ThenBy(app => app.Name);
    Display(SortedResault);
}

static void DivideData(IEnumerable<GoogleApp> googleApps)
{
    var hightRatedBeautyApps = googleApps.Where(app => app.Rating > 4.6 && app.Category == Category.BEAUTY);
    //var first5hightRatedBeautyApps = new List<GoogleApp>();

    //foreach(var app in hightRatedBeautyApps)
    //{
    //    first5hightRatedBeautyApps.Add(app);
    //    if(first5hightRatedBeautyApps.Count == 5) // jakbyśmy nie używali LINQ tak ograniczylibyśmy liczbe rekordów do 5 (without LINQ)
    //    {
    //        break;
    //    }
    //}

    // var first5hightRatedBeautyApps = hightRatedBeautyApps.TakeWhile(app => app.Reviews > 1000);

    var skippedResults = hightRatedBeautyApps.Skip(5);
    Console.WriteLine("Skipped Results:");

    Display(skippedResults);
}

    static void Displayy(GoogleApp googleApp)
    {
        Console.WriteLine(googleApp);
    }

    static List<GoogleApp> LoadGoogleAps(string csvpath) // Get the CSV file information 
    {
        using (var reader = new StreamReader(csvpath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<GoogleAppMap>();
            var records = csv.GetRecords<GoogleApp>().ToList();
            return records;
        }
    }


//OrderData(googleApps); // --> Where, OrderBy, OrderByDescending etc.
//DivideData(googleApps); // --> Take, Skip, TakeWhile etc.
//ProjectData(googleApps); // --> Select, Where, First, Last etc.
//Display(googleApps); // --> foreach :)
//GetData(googleApps); // --> First, Last etc.
DataSetOperation(googleApps); // --> Distinct, Union, Intersect, Except.