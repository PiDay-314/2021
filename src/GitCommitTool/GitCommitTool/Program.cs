using LibGit2Sharp;
using System;
using System.IO;

namespace GitCommitTool
{

    class Program
    {
        const string AsciArt = @"
xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
x      oooo       oo   o  o                        x
x         o        o   o  o                        x
x       ooo        o   ooooo                       x
x         o  oo    o      o                        x
x      oooo   o    o      o   o   o   o            x
xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
        const string PiDecimals = "14159265358979323846264338327950288419716939937510582097494459230781640628620899862803482534211706798214808651328230664709384460955058223172535940812848111745028410270193852110555964462294895493038196442881097566593344612847564823378678316527120190914564856692346034861045432664821339360726024914127372458700660631558817488152092096282925409171536436789259036001133053054882046652138414695194151160943305727036575959195309218611738193261179310511854807446237996274956735188575272489122793818301194912983367336244065664308602139494639522473719070217986094370277053921717629317675238467481846766940513200056812714526356082778577134275778960917363717872146844090122495343014654958537105079227968925892354201995611212902196086403441815981362977477130996051870721134999999837297804995105973173281609631859502445945534690830264252230825334468503526193118817101000313783875288658753320838142061717766914730359825349042875546873115956286388235378759375195778185778053217122680661300192787661119590921642019893809525720106548586327886593615338182796823030195203530185296899577362259941389124972177528347913151557485724245415069595082953311686172785588907509838175463746493931925506040092770167113900984882401285836160356370766010471018194295559619894676783744944825537977472684710404753464620804668425906949129331367702898915210475216205696602405803815019351125338243003558764024749647326391419927260426992279678235478163600934172164121992458631503028618297455570674983850549458858692699569092721079750930295532116534498720275596023648066549911988183479775356636980742654252786255181841757467289097777279380008164706001614524919217321721477235014144197356854816136115735255213347574184946843852332390739414333454776241686251898356948556209921922218427255025425688767179049460165346680498862723279178608578438382796797668145410095388378636095068006422512520511739298489608412848862694560424196528502221066118630674427862203919494504712371378696095636437191728746776465757396241389086583264599581339047802759009";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, I'll deploy this ASCII Art to github if you provided a correct repository in args!");
            var directory = Path.Combine(Path.GetTempPath(), "Pi Day", Guid.NewGuid().ToString());
            Directory.CreateDirectory(directory);
            var asciArt = AsciArt.Trim().Split(Environment.NewLine);
            var repositoryUrl = args[0];
            var repositoryPath = Repository.Clone(repositoryUrl, directory);
            var repo = new Repository(repositoryPath);
            var fileName = Path.Combine(directory, "pi");
            File.WriteAllText(fileName, "3,");
            var remainingPiDecimals = PiDecimals;
            for (var indexWeeks = 0; indexWeeks < 52; indexWeeks++)
            {
                for (var indexDays = 0; indexDays < 7; indexDays++)
                {
                    var currentChar = asciArt[indexDays][indexWeeks];
                    if (currentChar == ' ' || currentChar == 'x')
                    {
                        continue;
                    }
                    var diffDays = indexDays - 7;
                    var diffWeeks = indexWeeks - 51;
                    var totalDiffDays = diffDays + (7 * diffWeeks);
                    Commit(currentChar == '0' ? 9 : 3, repo, fileName, DateTimeOffset.UtcNow.AddDays(totalDiffDays), ref remainingPiDecimals);
                }
            }
            repo.Network.Push(repo.Branches["main"]);
        }

        static void Commit(int numberOfTimes, Repository repository, string fileName, DateTimeOffset time, ref string remainingPiDecimals)
        {
            for (var commitIndex = 0; commitIndex < numberOfTimes; commitIndex++)
            {
                var currentPiDecimal = remainingPiDecimals[0].ToString();
                remainingPiDecimals = remainingPiDecimals[1..];
                Commit(repository, fileName, currentPiDecimal, time);
            }
        }

        static void Commit(Repository repository, string fileName, string content, DateTimeOffset time)
        {
            File.AppendAllText(fileName, content);
            Commands.Stage(repository, fileName);
            repository.Commit("Tried to to a commit last yesterday", new Signature("Pi", "piday@shane.ro", time), new Signature("Pi", "piday@shane.ro", time));
        }
    }
}
