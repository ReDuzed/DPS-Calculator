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
            record = "record";
        private static int 
            wepAvg = 5, offhandAvg = 0;
        private static float 
            primaryStat = 1f, elemental = 1f, skill = 1f, critChance = 0.10f, critDamage = 1.50f, baseSpeed = 1, attackSpeed = 0f, bonusDmg = 1, gearDmg = 1f, bonusWep = 1, critProduct = 1.05f;
        private const byte
            Low = 0, High = 1, Elemental = 2, SKill = 3, CritChance = 4, CritDamage = 5, BaseSpeed= 6, AttackSpeed = 7, BonusDmg = 8;
        static void Main(string[] args)
        {
            string[] values = new string[] { exit, quit, all, mainhand, offhand, stat, crit, element, skillDmg, atkSpeed, gear, bonus, help, "?" };
            bool start = true;
            bool allStats = false;
            bool cmdList = false;
            string input = "";
            string notice = "";
            string w = "";
            int wep1, wep2;
            while (true)
            {
                Console.Clear();
                if (notice.Length > 4)
                    Console.WriteLine(notice + "\n");
                if (cmdList)
                    Console.Write(string.Format("Commands:{0}{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}{0}{13}{0}{14}{0}{15}{0}{16}{0}\n", new object[] { "\n   ", exit, quit, title, record, help, "?", all, mainhand, offhand, stat, crit, element, skillDmg, atkSpeed, gear, bonus }));
                Console.WriteLine(Output(start));
                string statList = "\n" +
                        "Main hand:       " + wepAvg + "\n" + 
                        "Off-hand:        " + offhandAvg + "\n" + 
                        "Primary stat:    " + primaryStat * 100 + "\n" + 
                        "Crit %:          " + critChance + "\n" + 
                        "Crit damage:     " + critDamage + "\n" + 
                        "Elemental %:     " + ((Math.Round(elemental, 0) - 1f) * 100f) + "\n" + 
                        "Skill damage %:  " + (skill * 100f) + "\n" + 
                        "Weapon speed:    " + baseSpeed + "\n" + 
                        "Attack speed %:  " + (attackSpeed * 100f) + "\n" + 
                        "Gear % bonus:    " + (Math.Round(gearDmg, 0) * 100f);
                if (allStats)
                    Console.WriteLine(statList);
                start = false;
                input = Console.ReadLine();
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i] == input)
                        break;
                    if (i == values.Length - 1)
                       continue;
                }
                
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
                        Console.WriteLine("Input title of this window (maybe name of build)");
                        Console.Title = Console.ReadLine();
                        break;
                    case record:
                        Console.WriteLine("Input name of file (perhaps the build name)");
                        w = Console.ReadLine();
                        using (StreamWriter sw = new StreamWriter(w + ".txt"))
                        {
                            sw.WriteLine(w);
                            sw.WriteLine(Output());
                            sw.WriteLine(statList);
                        }
                        break;
                    case mainhand:
                        Console.WriteLine("Input min and max values for main hand, separated by a dash.");
                        w = Console.ReadLine();
                        int.TryParse(w.Substring(0, w.IndexOf('-')), out wep1);
                        int.TryParse(w.Substring(w.IndexOf('-') + 1), out wep2);
                        wepAvg = (wep1 + wep2) / 2;
                        if (wepAvg == 0)
                            notice = "Notice: main hand weapon values at 0.";
                        break;
                    case offhand:
                        Console.WriteLine("Input min and max values for off hand, separated by a dash.");
                        w = Console.ReadLine();
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
                        critProduct = (critDamage / 100f + 1) * critChance / 100;
                        break;
                    case element:
                        Console.WriteLine("Input elemental damage multiplier.");
                        w = Console.ReadLine();
                        float.TryParse(w, out elemental);
                        elemental = elemental / 100f + 1;
                        break;
                    case skillDmg:
                        Console.WriteLine("Input skill damage multiplier.");
                        w = Console.ReadLine();
                        float.TryParse(w, out skill);
                        skill = skill / 100f + 1;
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
                        gearDmg /= 100f + 1;
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
                }
            }
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
            output += Format(wep, " base damage");
            float speed = baseSpeed * (attackSpeed + 1);
            output += Format (speed, " attacks per second");
            double product = wep * bonusWep * skill * speed;
            output += Format (Math.Round(product, 2), " post skill damage");
            double sheet = wep * speed * primaryStat * critProduct;
            output += Format(Math.Round(sheet, 2), " sheet damage");
            product *= primaryStat * critProduct * gearDmg;
            output += Format(Math.Round(product, 2), " average damage output per hit", false);
            return output;
        }
        private static string Format(object o, string text, bool newLine = true)
        {
            return string.Format("{0} {1}{2}", new object[] { o, text, newLine ? "\n" : "" });
        }
    }
}
