using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StatisticalLanguageModeling
{
    class Program
    {
        public static Dictionary<string, int> WordCountHash { get; set; }

        public static string[] WordGivenIndex { get; set; }

        public static Dictionary<string, int> IndexGivenWord  { get; set; }

        public static Dictionary<KeyValuePair<int, int>, int> Bigrams { get; set; }

        public static long WordTotal { get; set; }


        public static void Main(string[] args)
        {
            ImportData();
            //PrintPartA();
            //PrintPartB();
            //PrintPartC();
            //PrintPartD();
            //PrintPartE();
        }

        /**
         * HELPER AND CALCULATION FUNCTIONS:
         */

        private static void ImportData()
        {
            WordCountHash = new Dictionary<string, int>();
            WordGivenIndex = new string[501];
            IndexGivenWord = new Dictionary<string, int>();
            Bigrams = new Dictionary<KeyValuePair<int, int>, int>();
            var i = 1;
            string x;
            
            using (StreamReader s = new StreamReader(@"C:\Users\Will\Documents\Visual Studio 2012\Projects\StatisticalLanguageModeling\StatisticalLanguageModeling\DataFiles\vocab.txt"))
                using (StreamReader r = new StreamReader(@"C:\Users\Will\Documents\Visual Studio 2012\Projects\StatisticalLanguageModeling\StatisticalLanguageModeling\DataFiles\unigram.txt"))
                    while (!s.EndOfStream)
                    {
                        WordCountHash.Add((x = s.ReadLine()), Int32.Parse(r.ReadLine()));
                        IndexGivenWord[x] = i;
                        WordGivenIndex[i++] = x;
                    }

            using (StreamReader s = new StreamReader(@"C:\Users\Will\Documents\Visual Studio 2012\Projects\StatisticalLanguageModeling\StatisticalLanguageModeling\DataFiles\bigram.txt"))
                while (!s.EndOfStream)
                {
                    var tokens = s.ReadLine().Split('\t');
                    Bigrams.Add(new KeyValuePair<int, int>(Int32.Parse(tokens[1]), Int32.Parse(tokens[0])), Int32.Parse(tokens[2]));

                    
                }

            foreach(var pair in WordCountHash)
                WordTotal += pair.Value;
        }

        // count(w)
        private static int CountOfWord(string word)
        {
            return WordCountHash[word];
        }

        // count(w',w)
        private static int CountOfBigram(string word, string previousWord)
        {
            if ((!IndexGivenWord.ContainsKey(word)) || (!IndexGivenWord.ContainsKey(previousWord)))
                return 0;

            var x = new KeyValuePair<int, int>(IndexGivenWord[word], IndexGivenWord[previousWord]);
            return !Bigrams.ContainsKey(x) ? 0 : Bigrams[x];
           
        }

        // Pu(w) = count(w)/Sum count(w')
        private static double P_Unigram(string word)
        {
            return ((double)(CountOfWord(word)))/WordTotal;
        }

        // Pb(w'|w) = count(w',w) / count(w)    w is previousWord
        private static double P_Bigram(string word, string previousWord)
        {
            return ((double)CountOfBigram(word,previousWord))/CountOfWord(previousWord);
        }


        // Pmix(w'|w) = (1-Lambda)Pu(w') + (Lambda)Pb(w'|w)
        private static double P_Mix(string word, string previousWord, double lambda)
        {
            return (1-lambda)*(P_Unigram(word)) + (lambda)*(P_Bigram(word,previousWord));
        }


        /**
         * OUTPUT FUNCTIONS:
         */


        /************************************************************************
         PART A: Compute maximum likelihood estimate of the unigram distribution
                   Pu(w) over words w. 
            
                   Prints out a table of all words w that start with 'A' along with
                   their unigram probabilities: Pu(w)                           
        /************************************************************************/
        public static void PrintPartA()
        {
            foreach (var pair in WordCountHash.Where(pair => pair.Key[0] == 'A'))
                Console.WriteLine("P({0})= {1}", pair.Key, P_Unigram(pair.Key));
        }
        #region Part A:
        /*
            P(A) = 0.0184072446907125
            P(AND) = 0.0178632339250206
            P(AT) = 0.00431297400061244
            P(AS) = 0.00399179716740647
            P(AN) = 0.00299925667394354
            P(ARE) = 0.00298969267091369
            P(ABOUT) = 0.00192561783765327
            P(AFTER) = 0.00134656759794536
            P(ALSO) = 0.0013100115812494
            P(ALL) = 0.00118181480406403
            P(A.) = 0.00102561090803164
            P(ANY) = 0.000631860169481472
            P(AMERICAN) = 0.000612096193910822
            P(AGAINST) = 0.000595964582662253
            P(ANOTHER) = 0.000428386616530418
            P(AMONG) = 0.000374292517552086
            P(AGO) = 0.000356570982526175
            P(ACCORDING) = 0.000347545107544034
            P(AIR) = 0.000311001321030976
            P(ADMINISTRATION) = 0.000291518639667087
            P(AGENCY) = 0.000279655362251536
            P(AROUND) = 0.000276854650366833
            P(AGREEMENT) = 0.000262789940028809
            P(AVERAGE) = 0.000259071964426409
            P(ASKED) = 0.000258228081806128
            P(ALREADY) = 0.000249079904994961
            P(AREA) = 0.000231089305945192
            P(ANALYSTS) = 0.000226038240406406
            P(ANNOUNCED) = 0.000227151187050545
            P(ADDED) = 0.00022121954834277
            P(ALTHOUGH) = 0.000214260574271173
            P(AGREED) = 0.00021177784714194
            P(APRIL) = 0.000206690091054446
            P(AWAY) = 0.000202054851734349
         */
        #endregion



        /************************************************************************
         PART B: Compute the maximum likelihood estimate of the bigram distribution
                 Pb(w'|w).
            
                 Print out top 10 probabilities with Pb(w'|w='THE')                         
        /************************************************************************/
        public static void PrintPartB()
        {
            //List of Pb(w|THE)
            List<KeyValuePair<string,double>> probabilityList = new List<KeyValuePair<string, double>>();
            const string previousWord = "THE";
            int previousWordIndex = IndexGivenWord[previousWord]; // 1-based

            for (int i = 1; i < 501; i++)
            {
                double probability = P_Bigram(WordGivenIndex[i], previousWord);
                probabilityList.Add(new KeyValuePair<string, double>(WordGivenIndex[i], probability));
            }

            var SortedList = probabilityList.OrderBy(x => x.Value).ToList();
            SortedList.Reverse();

            for (int i = 0; i < 10; i++)
                Console.WriteLine(SortedList[i]);
        }
        #region Part B:
        /*
            [<UNK>, 0.615019810005512]
            [U., 0.0133724994326103]
            [FIRST, 0.0117202606750316]
            [COMPANY, 0.0116587880556366]
            [NEW, 0.00945148007651655]
            [UNITED, 0.0086723081412314]
            [GOVERNMENT, 0.0068034886359952]
            [NINETEEN, 0.00665071491100088]
            [SAME, 0.00628706675744902]
            [TWO, 0.00616074960282722]
         */
        #endregion


        /************************************************************************
         PART C: Compute and compare the Log-Likelihoods of the sentence “The 
                 stock market fell by one hundred points last week” using the unigram
                 and bigram models
         
           L-unigram = log[Pu(THE)*Pu(STOCK)*...*Pu(WEEK)]   = Sum log(THE) + log(STOCK)..
           L-bigram  = log[Pb(THE|<start>*...*Pb(WEEK|LAST)]
        /************************************************************************/
        public static void PrintPartC()
        {
            double unigram = Math.Log(P_Unigram("THE"))     + Math.Log(P_Unigram("STOCK"))  + Math.Log(P_Unigram("MARKET")) +
                             Math.Log(P_Unigram("FELL"))    + Math.Log(P_Unigram("BY"))     + Math.Log(P_Unigram("ONE"))    +
                             Math.Log(P_Unigram("HUNDRED")) + Math.Log(P_Unigram("POINTS")) + Math.Log(P_Unigram("LAST"))   + 
                             Math.Log(P_Unigram("WEEK"));

            double bigram  = Math.Log(P_Bigram("THE","<s>"))     + Math.Log(P_Bigram("STOCK","THE"))      + Math.Log(P_Bigram("MARKET","STOCK"))   + 
                             Math.Log(P_Bigram("FELL","MARKET")) + Math.Log(P_Bigram("BY","FELL"))        + Math.Log(P_Bigram("ONE","BY"))         +
                             Math.Log(P_Bigram("HUNDRED", "ONE"))+ Math.Log(P_Bigram("POINTS", "HUNDRED")) + Math.Log(P_Bigram("LAST", "POINTS")) +
                             Math.Log(P_Bigram("WEEK", "LAST"));

            Console.WriteLine("Unigram = {0} \nBigram = {1}", unigram, bigram);
        }
        #region Part C:
        /*
            Unigram = -64.5094403436488
            Bigram = -40.9181321337898
         
            Bigram > Unigram
         */
        #endregion



        /************************************************************************
         PART D: Compute and compare the Log-Likelihoods of the sentence “The 
                 sixteen officials sold fire insurance"
         
           L-unigram = log[Pu(THE)*Pu(STOCK)*...*Pu(WEEK)]   = Sum log(THE) + log(STOCK)..
           L-bigram  = log[Pb(THE|<start>*...*Pb(WEEK|LAST)]
        /************************************************************************/
        public static void PrintPartD()
        {
            double unigram = Math.Log(P_Unigram("THE")) + Math.Log(P_Unigram("SIXTEEN")) + Math.Log(P_Unigram("OFFICIALS")) +
                             Math.Log(P_Unigram("SOLD")) + Math.Log(P_Unigram("FIRE")) + Math.Log(P_Unigram("INSURANCE"));
            ;
            double bigram = Math.Log(P_Bigram("THE", "<s>")) + Math.Log(P_Bigram("SIXTEEN", "THE")) +
                            Math.Log(P_Bigram("OFFICIALS", "SIXTEEN")) +
                            Math.Log(P_Bigram("SOLD", "OFFICIALS")) + Math.Log(P_Bigram("FIRE", "SOLD")) +
                            Math.Log(P_Bigram("INSURANCE", "FIRE"));

            Console.WriteLine("Unigram = {0} \nBigram = {1}", unigram, bigram);
        }
        #region Part D:
        /*
            Unigram = -44.2919344731326
            Bigram = -Infinity
         
            Bigram < Unigram
            The Bigram model is -Infinity due to the the corpus not containing a particular bigram.
            P_Bigram("SOLD", "FIRE") == 0    and also     P_Bigram("FIRE", "INSURANCE") == 0
         */
        #endregion


        /************************************************************************
         PART E: Compute and compare the mix Log-Likelihood of the sentence “The 
                 sixteen officials sold fire insurance"
         
           L-mix = log(Pm(THE|start) + .. + log(Pm(INSURANCE|FIRE)
        /************************************************************************/
        public static void PrintPartE()
        {
            double[] probabilityList = new double[100];

            for (int i = 0; i < 100; i++)
            {
                double j = i/100.0;
                double mixture = Math.Log(P_Mix("THE", "<s>", j)) + Math.Log(P_Mix("SIXTEEN", "THE", j)) +
                                 Math.Log(P_Mix("OFFICIALS", "SIXTEEN", j)) +
                                 Math.Log(P_Mix("SOLD", "OFFICIALS", j)) + Math.Log(P_Mix("FIRE", "SOLD", j)) +
                                 Math.Log(P_Mix("INSURANCE", "FIRE", j));
                probabilityList[i] = mixture;
                Console.WriteLine("Lambda = {0} \t Pm = {1}", j, mixture);
            }

        }
        #region Part E:
         /*
            Lambda = 0       Pm = -44.2919344731326
            Lambda = 0.01    Pm = -44.1647794442511
            Lambda = 0.02    Pm = -44.0529525478015
            Lambda = 0.03    Pm = -43.9534602109713
            Lambda = 0.04    Pm = -43.8641381319573
            Lambda = 0.05    Pm = -43.7833700193852
            Lambda = 0.06    Pm = -43.7099164350302
            Lambda = 0.07    Pm = -43.6428057428703
            Lambda = 0.08    Pm = -43.581261934392
            Lambda = 0.09    Pm = -43.5246553042629
            Lambda = 0.1     Pm = -43.4724678058568
            Lambda = 0.11    Pm = -43.4242681343032
            Lambda = 0.12    Pm = -43.3796934310469
            Lambda = 0.13    Pm = -43.3384356031134
            Lambda = 0.14    Pm = -43.3002309261867
            Lambda = 0.15    Pm = -43.26485202821
            Lambda = 0.16    Pm = -43.2321016276637
            Lambda = 0.17    Pm = -43.201807584788
            Lambda = 0.18    Pm = -43.1738189487157
            Lambda = 0.19    Pm = -43.1480027694914
            Lambda = 0.2     Pm = -43.1242415042868
            Lambda = 0.21    Pm = -43.1024308900929
            Lambda = 0.22    Pm = -43.0824781862027
            Lambda = 0.23    Pm = -43.0643007125067
            Lambda = 0.24    Pm = -43.0478246264361
            Lambda = 0.25    Pm = -43.0329838939735
            Lambda = 0.26    Pm = -43.0197194196734
            Lambda = 0.27    Pm = -43.007978307906
            Lambda = 0.28    Pm = -42.9977132331416
            Lambda = 0.29    Pm = -42.9888819014504
            Lambda = 0.3     Pm = -42.9814465888039
            Lambda = 0.31    Pm = -42.9753737444627
            Lambda = 0.32    Pm = -42.9706336498788
            Lambda = 0.33    Pm = -42.9672001252601
            Lambda = 0.34    Pm = -42.9650502773324
            Lambda = 0.35    Pm = -42.964164282963  <----GLOBAL MAXIMUM
            Lambda = 0.36    Pm = -42.9645252042323
            Lambda = 0.37    Pm = -42.9661188313049
            Lambda = 0.38    Pm = -42.9689335500831
            Lambda = 0.39    Pm = -42.9729602321601
            Lambda = 0.4     Pm = -42.9781921450418
            Lambda = 0.41    Pm = -42.9846248809909
            Lambda = 0.42    Pm = -42.9922563031863
            Lambda = 0.43    Pm = -43.0010865081836
            Lambda = 0.44    Pm = -43.0111178039309
            Lambda = 0.45    Pm = -43.0223547028343
            Lambda = 0.46    Pm = -43.0348039295978
            Lambda = 0.47    Pm = -43.0484744437742
            Lambda = 0.48    Pm = -43.0633774771808
            Lambda = 0.49    Pm = -43.0795265865435
            Lambda = 0.5     Pm = -43.0969377219561
            Lambda = 0.51    Pm = -43.1156293119691
            Lambda = 0.52    Pm = -43.135622366376
            Lambda = 0.53    Pm = -43.1569405980359
            Lambda = 0.54    Pm = -43.179610565376
            Lambda = 0.55    Pm = -43.2036618375663
            Lambda = 0.56    Pm = -43.2291271847498
            Lambda = 0.57    Pm = -43.2560427961711
            Lambda = 0.58    Pm = -43.2844485295841
            Lambda = 0.59    Pm = -43.3143881959418
            Lambda = 0.6     Pm = -43.3459098841262
            Lambda = 0.61    Pm = -43.3790663313621
            Lambda = 0.62    Pm = -43.4139153460324
            Lambda = 0.63    Pm = -43.4505202909061
            Lambda = 0.64    Pm = -43.4889506363657
            Lambda = 0.65    Pm = -43.5292825951434
            Lambda = 0.66    Pm = -43.5715998524438
            Lambda = 0.67    Pm = -43.6159944082614
            Lambda = 0.68    Pm = -43.6625675523472
            Lambda = 0.69    Pm = -43.7114309968525
            Lambda = 0.7     Pm = -43.7627081974492
            Lambda = 0.71    Pm = -43.816535901056
            Lambda = 0.72    Pm = -43.8730659676949
            Lambda = 0.73    Pm = -43.9324675261248
            Lambda = 0.74    Pm = -43.9949295386848
            Lambda = 0.75    Pm = -44.0606638715207
            Lambda = 0.76    Pm = -44.1299089938896
            Lambda = 0.77    Pm = -44.2029344671308
            Lambda = 0.78    Pm = -44.2800464339283
            Lambda = 0.79    Pm = -44.361594387157
            Lambda = 0.8     Pm = -44.4479795931198
            Lambda = 0.81    Pm = -44.5396656787369
            Lambda = 0.82    Pm = -44.6371920853881
            Lambda = 0.83    Pm = -44.7411913737122
            Lambda = 0.84    Pm = -44.8524117821164
            Lambda = 0.85    Pm = -44.9717470767122
            Lambda = 0.86    Pm = -45.1002767166838
            Lambda = 0.87    Pm = -45.2393209317433
            Lambda = 0.88    Pm = -45.3905178912312
            Lambda = 0.89    Pm = -45.5559345318962
            Lambda = 0.9     Pm = -45.738230358733
            Lambda = 0.91    Pm = -45.9409078443309
            Lambda = 0.92    Pm = -46.168710932033
            Lambda = 0.93    Pm = -46.4282910397553
            Lambda = 0.94    Pm = -46.7293899416097
            Lambda = 0.95    Pm = -47.0871109015392
            Lambda = 0.96    Pm = -47.5267564113155
            Lambda = 0.97    Pm = -48.0957599549459
            Lambda = 0.98    Pm = -48.9006111685963
            Lambda = 0.99    Pm = -50.2811089119932
         */
        #endregion

    }
}
