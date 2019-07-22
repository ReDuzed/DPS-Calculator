using System;
using System.IO;

namespace planner
{
    class Program
    {
        private const string 
            help = "help",
            exit = "exit",
            quit = "quit",
            mainhand = "wep1",
            offhand = "wep2",
            stat = "stat",
            crit = "crit",
            element = "element",
            skillDmg = "skill",
            atkSpeed = "ias",
            gear = "gear",
            bonus = "wep%",
            all = "all",
            title = "title",
            record = "record",
            load = "load",
            grift = "gr",
            atkRate = "atkrate";
        private static int 
            wepAvg = 5, offhandAvg = 0, tier = 1;
        private static float 
            primaryStat = 1f, elemental = 1f, skill = 1f, critChance = 0.10f, critDamage = 1.50f, baseSpeed = 1, attackSpeed = 0f, bonusDmg = 1, gearDmg = 1f, bonusWep = 1, critProduct = 1.15f, hitRate = 1f;
        private const byte
            Low = 0, High = 1, Elemental = 2, SKill = 3, CritChance = 4, CritDamage = 5, BaseSpeed= 6, AttackSpeed = 7, BonusDmg = 8;
        private static double dmgProduct;
        ///<summary>
        ///5 elites, 4 champion packs, [?]4 mobs
        ///</summary>
        private const int foeCount = 17;
        static class GR
        {
            public const float 
            whiteBaseHP = 1737040f,
            blueBaseHp = 5177516,
            yellowBaseHP = 7104020f,
            scale1 = 1.17f,
            scale5 = 2.1924f;
        }
        static void Main(string[] args)
        {
            string[] values = new string[] { exit, quit, all, mainhand, offhand, stat, crit, element, skillDmg, atkSpeed, gear, bonus, help, "?", record, load, title, grift, atkRate };
            bool start = true;
            bool allStats = false;
            bool cmdList = false;
            bool grStats = false;
            string input = "";
            string notice = "";
            string w = "";
            int wep1, wep2;
            float[] array;
            while (true)
            {
                Console.Clear();
                if (notice.Length > 4)
                    Console.WriteLine(notice + "\n");
                notice = "";
                if (cmdList)
                    Console.Write(string.Format("Commands:{0}{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}{0}{13}{0}{14}{0}{15}{0}{16}{0}{17}{0}{18}{0}{19}{0}\n", new object[] { "\n   ", exit, quit, title, record, load, help, "?", grift, all, mainhand, offhand, stat, crit, element, skillDmg, atkSpeed, gear, bonus, atkRate }));
                Console.WriteLine(Output(start));
                string statList = "\n" +
                        "Main hand:         " + wepAvg + "\n" + 
                        "Off-hand:          " + offhandAvg + "\n" + 
                        "Primary stat:      " + (primaryStat * 100 - 100) + "\n" + 
                        "Crit %:            " + critChance + "\n" + 
                        "Crit damage %:     " + critDamage + "\n" + 
                        "Elemental %:       " + ((Math.Round(elemental, 2) - 1f) * 100f) + "\n" + 
                        "Skill damage %:    " + (skill * 100f - 100) + "\n" + 
                        "Weapon speed:      " + baseSpeed + "\n" + 
                        "Attack speed %:    " + (attackSpeed * 100f) + "\n" + 
                        "Gear % bonus:      " + (Math.Round(gearDmg, 2) * 100f - 100) + "\n" +
                        "Weapon % bonus:    " + (bonusWep * 100f) + "\n" +
                        "Skill atk. rate:   " + hitRate;
                if (allStats)
                    Console.WriteLine(statList);
                if (grStats)
                {
                    double scale = GrHpScale(tier) / 100d + 1,
                        white = scale * GR.whiteBaseHP,
                        blue = scale * GR.blueBaseHp,
                        yellow = scale * GR.yellowBaseHP,
                        sAverage = (blue + yellow) / 2d,
                        nTtk = white / dmgProduct,
                        sTtk = sAverage / dmgProduct,
                        time = (sTtk * foeCount - 4) + (nTtk * 4);
                    string timeSpan = "";
                    try
                    {
                        timeSpan += Math.Round((time / 60d) * 1.08f, 0);
                    }
                    catch
                    {
                        timeSpan = "NaN";
                    }
                    Console.WriteLine("\n" +
                        "Greater rift lvl.: " + tier + "\n" +
                        "Health scale %:    " + (Math.Round(scale, 2) * 100d) + "\n" +
                        "Normal health:     " + Math.Round(white, 0) + "\n" +
                        "   TTK:            " + (int)nTtk + " seconds\n" +
                        "Champion health:   " + Math.Round(blue, 0) + "\n" + 
                        "Elite health:      " + Math.Round(yellow, 0) + "\n" + 
                        "   TTK average:    " + (int)sTtk + " seconds\n" +
                        "Est. clear time:   " + timeSpan + " minutes");
                }
                start = false;
                input = Console.ReadLine();
                bool dev = true;
                if (input == grift && !dev)
                    continue;
                
                switch (input)
                {
                    case quit:
                        return;
                    case exit:
                        goto case quit;
                    case help:
                        cmdList = !cmdList;
                        break;
                    case "?":
                        goto case help;
                    case title:
                        Console.WriteLine("Input title of this window (maybe name of build).");
                        Console.Title = Console.ReadLine();
                        break;
                    case record:
                        Console.WriteLine("Input name of file (perhaps the build name).");
                        w = Console.ReadLine();
                        if (!Directory.Exists("builds"))
                            Directory.CreateDirectory("builds");
                        if (!Directory.Exists("saves"))
                            Directory.CreateDirectory("saves");
                        StreamWriter sw = new StreamWriter("builds\\" + w + ".txt");
                        sw.WriteLine(w);
                        sw.WriteLine(Output());
                        sw.WriteLine(statList);
                        sw.Flush();
                        sw = new StreamWriter("saves\\" + w + ".txt");
                        array = new float[] 
                        { 
                            wepAvg,         offhandAvg,
                            primaryStat,    elemental,  
                            skill,          critChance, 
                            critDamage,     baseSpeed,  
                            attackSpeed,    bonusDmg,   
                            gearDmg,        bonusWep,
                            hitRate
                        };
                        foreach (float f in array)
                            sw.WriteLine(f + ",");
                        sw.Flush();
                        sw.Dispose();
                        break;
                    case load:
                        if (!Directory.Exists("saves"))
                            Directory.CreateDirectory("saves");
                        string[] saves = Directory.GetFileSystemEntries("saves");
                        if (saves.Length == 0)
                        {
                            notice = "Notice: no builds saved";
                            break;
                        }
                        Console.WriteLine("Input name of file to load from (maybe name of build).");
                        foreach (string c in saves)
                            Console.WriteLine(c.Substring(c.IndexOf('\\') + 1).Replace(".txt", ""));
                        w = Console.ReadLine();
                        string file = "saves\\" + w + ".txt";
                        if (w.Length < 2 || !File.Exists(file))
                            continue;
                        using (StreamReader sr = new StreamReader(file))
                        {
                            int index = 0;
                            array = new float[14];
                            foreach (string s in sr.ReadToEnd().Split(','))
                                float.TryParse(s, out array[index++]);
                            wepAvg = (int)array[0]; 
                            offhandAvg = (int)array[1];
                            primaryStat = array[2];
                            elemental = array[3];
                            skill = array[4];
                            critChance = array[5];
                            critDamage = array[6];
                            critProduct = (critDamage / 100 + 1) * (critChance / 100);
                            baseSpeed = array[7];
                            attackSpeed = array[8];
                            bonusDmg = array[9]; 
                            gearDmg = array[10]; 
                            bonusWep = array[11];
                            hitRate = array[12];
                        }
                        break;
                    case mainhand:
                        Console.WriteLine("Input min and max values for main hand, separated by a dash.");
                        w = Console.ReadLine();
                        if (!w.Contains("-"))
                        {
                            notice = "Notice: range not formatted correctly";
                            break;
                        }
                        int.TryParse(w.Substring(0, w.IndexOf('-')), out wep1);
                        int.TryParse(w.Substring(w.IndexOf('-') + 1), out wep2);
                        wepAvg = (wep1 + wep2) / 2;
                        if (wepAvg == 0)
                            notice = "Notice: main hand weapon values at 0.";
                        break;
                    case offhand:
                        Console.WriteLine("Input min and max values for off hand, separated by a dash.");
                        w = Console.ReadLine();
                        if (!w.Contains("-"))
                        {
                            notice = "Notice: range not formatted correctly";
                            break;
                        }
                        int.TryParse(w.Substring(0, w.IndexOf('-')), out wep1);
                        int.TryParse(w.Substring(w.IndexOf('-') + 1), out wep2);
                        offhandAvg = (wep1 + wep2) / 2;
                        if (wepAvg == 0)
                            notice = "Notice: off hand weapon values at 0.";
                        break;
                    case stat:
                        Console.WriteLine("Input primary stat value.");
                        w = Console.ReadLine();
                        if (!float.TryParse(w, out primaryStat))
                            notice = "Notice: primary stat input invalid.";
                        primaryStat = primaryStat / 100f + 1;
                        break;
                    case crit:
                        Console.WriteLine("Input crit chance.");
                        w = Console.ReadLine();
                        float.TryParse(w, out critChance);
                        Console.WriteLine("Input crit damage.");
                        w = Console.ReadLine();
                        float.TryParse(w, out critDamage);
                        critProduct = (critDamage / 100 + 1) * (critChance / 100);
                        break;
                    case element:
                        Console.WriteLine("Input elemental damage multiplier.");
                        w = Console.ReadLine();
                        float.TryParse(w, out elemental);
                        elemental /= 100f;
                        elemental += 1f;
                        break;
                    case skillDmg:
                        Console.WriteLine("Input skill damage multiplier.");
                        w = Console.ReadLine();
                        float.TryParse(w, out skill);
                        skill /= 100f;
                        skill += 1;
                        break;
                    case atkSpeed:
                        Console.WriteLine("Input base weapon speed.");
                        w = Console.ReadLine();
                        float.TryParse(w, out baseSpeed);
                        Console.WriteLine("Input attack speed multiplier.");
                        w = Console.ReadLine();
                        float.TryParse(w, out attackSpeed);
                        attackSpeed /= 100f;
                        break;
                    case gear:
                        Console.WriteLine("Input gear damage multiplier.");
                        w = Console.ReadLine();
                        float.TryParse(w, out gearDmg);
                        gearDmg /= 100f;
                        gearDmg += 1f;
                        break;
                    case bonus:
                        Console.WriteLine("Input bonus weapon damage percent.");
                        w = Console.ReadLine();
                        float.TryParse(w, out bonusWep);
                        bonusWep /= 100f;
                        break;
                    case all:
                        allStats = !allStats;
                        break;
                    case grift:
                        notice = "Notice: the time is estimated using the total of " + foeCount + " averaged foes' health (with an 8% margin of error added to clear time).";
                        Console.WriteLine("Input greater rift tier between 1-150 (0 to make information hidden).");
                        if (!int.TryParse(Console.ReadLine(), out tier))
                            notice = "Invalid tier input.";
                        else 
                            grStats = tier > 0 && tier <= 150 ? true : false;
                        break;
                    case atkRate:
                        Console.WriteLine("Input hit rate of skill (hit per second) if gear autocasts or skill cast speed isn't based on IAS.");
                        w = Console.ReadLine();
                        float.TryParse(w, out hitRate);
                        break;
                }
            }
        }
        private static double GrHpScale(int tier)
        {
            return (tier < 5 ? 1d : 200d) * Math.Pow(GR.scale1, Math.Max(tier - 1, 1)) - (tier == 5 ? ((tier - 4) * 275) : 1);
        }
        private static string Output(bool start = false)
        {
            if (start)
            {
                return "Input 'help' or '?' for available commands\n"
                +"Hit enter to continue...";
            }
            string output = "";
            float wep = wepAvg + offhandAvg;
            output += Format(wep, "base damage");
            float speed = baseSpeed * (attackSpeed + 1);
            output += Format(speed, "attacks per second");
            double sheet = wep * speed * primaryStat * critProduct;
            output += Format(Math.Round(sheet, 2), "sheet damage");
            double product = sheet * skill;
            output += Format (Math.Round(product, 0), "post skill damage");
            product *= gearDmg * elemental * bonusWep;
            output += Format((dmgProduct = Math.Round(product, 2)), "average damage output per hit", hitRate != 1f);
            if (hitRate != 1f)
            {
                product *= hitRate;
                dmgProduct *= hitRate;
                output += Format(Math.Round(product, 2), "estimated damage of skill per second", false);
            }
            return output;
        }
        private static string Format(object o, string text, bool newLine = true)
        {
            string i = o.ToString();
            int length = i.Length;
            while (length++ < 18)
                i += " ";
            string t = string.Format("{0} {1}{2}", new object[] { i, text, newLine ? "\n" : "" });
            return t;
        }
    }
}
