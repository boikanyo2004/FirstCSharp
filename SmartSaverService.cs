using System;
using System.Collections.Generic;

namespace WeatherApp
{
    public class SmartSaverService
    {
        // Get all savings opportunities for the current weather
        public static List<SavingsOpportunity> GetSavingsOpportunities(WeatherData weather)
        {
            var opportunities = new List<SavingsOpportunity>();
            double temp = weather.Temperature;
            string condition = weather.WeatherCondition?.ToLower() ?? "";
            int humidity = weather.Humidity;
            double windSpeed = weather.WindSpeed;

            // Sunny weather opportunities
            if (condition.Contains("clear") || condition.Contains("sun"))
            {
                opportunities.Add(new SavingsOpportunity
                {
                    Icon = "☀️",
                    Title = "DRY CLOTHES OUTSIDE",
                    Description = "Instead of: Electric dryer (3 loads)",
                    MoneySaved = 2.55,
                    EnergySaved = 7.5,
                    CO2Saved = 5.4,
                    Reason = "Perfect sunny weather for natural drying!"
                });

                opportunities.Add(new SavingsOpportunity
                {
                    Icon = "💡",
                    Title = "USE NATURAL LIGHT",
                    Description = "Instead of: Indoor lights (until sunset)",
                    MoneySaved = 0.80,
                    EnergySaved = 2.4,
                    CO2Saved = 1.7,
                    Reason = "Bright sunshine means free natural lighting!"
                });
            }

            // Good weather for walking/biking
            if (temp > 10 && temp < 28 && !condition.Contains("rain") && !condition.Contains("storm"))
            {
                opportunities.Add(new SavingsOpportunity
                {
                    Icon = "🚶",
                    Title = "WALK OR BIKE TODAY",
                    Description = "Instead of: Driving (10km round trip)",
                    MoneySaved = 3.20,
                    EnergySaved = 15.0,
                    CO2Saved = 2.4,
                    Reason = "Perfect weather for outdoor activity!"
                });
            }

            // Hot weather - natural cooling opportunities
            if (temp >= 20 && temp < 30)
            {
                opportunities.Add(new SavingsOpportunity
                {
                    Icon = "💨",
                    Title = "OPEN WINDOWS (SKIP AC)",
                    Description = "Instead of: Air conditioning (6 hours)",
                    MoneySaved = 1.50,
                    EnergySaved = 4.2,
                    CO2Saved = 3.1,
                    Reason = "Pleasant temperature - natural ventilation works!"
                });
            }
            else if (temp >= 30)
            {
                opportunities.Add(new SavingsOpportunity
                {
                    Icon = "🌡️",
                    Title = "STRATEGIC COOLING",
                    Description = "Close blinds during peak hours, use fans instead of AC",
                    MoneySaved = 3.80,
                    EnergySaved = 10.5,
                    CO2Saved = 7.5,
                    Reason = "Reduce AC usage with smart cooling strategies"
                });

                opportunities.Add(new SavingsOpportunity
                {
                    Icon = "🥗",
                    Title = "COLD MEALS TODAY",
                    Description = "Instead of: Using oven/stove (2 hours)",
                    MoneySaved = 0.65,
                    EnergySaved = 1.8,
                    CO2Saved = 1.3,
                    Reason = "Skip heating up your kitchen in this heat!"
                });
            }

            // Cold weather - heating savings
            if (temp < 15)
            {
                opportunities.Add(new SavingsOpportunity
                {
                    Icon = "🧥",
                    Title = "LAYER UP, HEAT DOWN",
                    Description = "Lower thermostat by 2°C, wear extra layer",
                    MoneySaved = 2.10,
                    EnergySaved = 6.0,
                    CO2Saved = 4.2,
                    Reason = "Stay warm naturally and save on heating!"
                });
            }

            if (temp < 15 && (condition.Contains("clear") || condition.Contains("sun")))
            {
                opportunities.Add(new SavingsOpportunity
                {
                    Icon = "🌞",
                    Title = "SOLAR HEATING",
                    Description = "Open curtains on sunny side for passive heating",
                    MoneySaved = 1.50,
                    EnergySaved = 4.5,
                    CO2Saved = 3.2,
                    Reason = "Free warmth from the sun!"
                });
            }

            // Rainy weather opportunities
            if (condition.Contains("rain") || condition.Contains("drizzle"))
            {
                opportunities.Add(new SavingsOpportunity
                {
                    Icon = "💧",
                    Title = "SKIP LAWN WATERING",
                    Description = "Nature's watering your garden for free!",
                    MoneySaved = 0.40,
                    EnergySaved = 1.2,
                    CO2Saved = 0.8,
                    Reason = "Rain = free irrigation!"
                });

                opportunities.Add(new SavingsOpportunity
                {
                    Icon = "🏠",
                    Title = "WORK FROM HOME",
                    Description = "Instead of: Commuting (20km round trip)",
                    MoneySaved = 6.40,
                    EnergySaved = 30.0,
                    CO2Saved = 4.8,
                    Reason = "Skip the wet commute, save big!"
                });

                opportunities.Add(new SavingsOpportunity
                {
                    Icon = "🌧️",
                    Title = "COLLECT RAINWATER",
                    Description = "Save 20-50L for future plant watering",
                    MoneySaved = 0.15,
                    EnergySaved = 0.5,
                    CO2Saved = 0.3,
                    Reason = "Free water for your plants!"
                });
            }

            // Windy weather
            if (windSpeed > 5)
            {
                opportunities.Add(new SavingsOpportunity
                {
                    Icon = "🌬️",
                    Title = "WIND-POWERED DRYING",
                    Description = "Dry clothes extra fast with natural wind",
                    MoneySaved = 2.55,
                    EnergySaved = 7.5,
                    CO2Saved = 5.4,
                    Reason = "Strong winds = super-fast drying!"
                });

                opportunities.Add(new SavingsOpportunity
                {
                    Icon = "💨",
                    Title = "NATURAL VENTILATION",
                    Description = "Skip electric fans, use natural airflow",
                    MoneySaved = 0.35,
                    EnergySaved = 1.0,
                    CO2Saved = 0.7,
                    Reason = "Free natural air circulation!"
                });
            }

            // Cold batch cooking opportunity
            if (temp < 10)
            {
                opportunities.Add(new SavingsOpportunity
                {
                    Icon = "🍲",
                    Title = "BATCH COOKING",
                    Description = "Cook multiple meals - oven warmth heats kitchen",
                    MoneySaved = 0.90,
                    EnergySaved = 2.7,
                    CO2Saved = 1.9,
                    Reason = "Double benefit: meals prepped + free heating!"
                });
            }

            return opportunities;
        }

        // Get monthly savings summary
        public static MonthlySummary GetMonthlySummary()
        {
            // This would normally come from a database tracking user's checked items
            // For now, we'll return example data
            return new MonthlySummary
            {
                TotalMoney = 87.50,
                TotalEnergy = 312.0,
                TotalCO2 = 156.0,
                DaysActive = 23,
                CurrentStreak = 7,
                TreesEquivalent = 7.8
            };
        }

        // Get fun fact about savings
        public static string GetSavingsFact(WeatherData weather)
        {
            var facts = new List<string>
            {
                "💡 If everyone in a city of 1 million dried clothes outside on sunny days, it would save $2.3 million and prevent 4,500 tons of CO₂ annually - equal to planting 225,000 trees!",
                "🚗 Walking just 10km per week instead of driving saves you $832 per year and prevents 384kg of CO₂ - that's like planting 19 trees!",
                "🌡️ Lowering your thermostat by just 1°C saves 10% on heating bills. That's about $120-180 per year for the average household!",
                "☀️ Using natural light instead of artificial lighting for just 3 hours a day saves $87 per year and prevents 60kg of CO₂!",
                "💧 A dripping tap wastes 20,000 liters per year - that's $45 down the drain! Fix those leaks!",
                "🌬️ Air-drying clothes instead of using a dryer saves the average family $200 per year and prevents 450kg of CO₂!",
                "🏠 Working from home just 2 days a week saves $2,600 per year in commuting costs and prevents 1,200kg of CO₂!",
                "💡 LED bulbs use 75% less energy than incandescent bulbs. Switching 20 bulbs saves $225 per year!",
                "🌳 One tree absorbs about 20kg of CO₂ per year. Your monthly savings are equivalent to planting a small forest!",
                "⚡ Unplugging devices when not in use (vampire power) saves the average household $165 per year!",
                "🚿 A 5-minute shower uses 40 liters less water than a bath. That's $120 saved per year for a family of 4!",
                "🌞 Solar heating can reduce water heating bills by 50-80%. That's $300-500 per year in savings!",
                "🍃 If every household replaced just one car trip per week with biking, the world would prevent 4 million tons of CO₂ annually!",
                "💨 Opening windows for natural cooling instead of AC for just summer weekends saves $200 and prevents 145kg of CO₂ per year!",
                "🌍 The average person can reduce their carbon footprint by 30% just by making weather-smart choices daily!"
            };

            Random random = new Random();
            return facts[random.Next(facts.Count)];
        }

        // Calculate total potential savings for today
        public static DailySummary CalculateDailySummary(List<SavingsOpportunity> opportunities)
        {
            var summary = new DailySummary();
            
            foreach (var opp in opportunities)
            {
                summary.TotalMoney += opp.MoneySaved;
                summary.TotalEnergy += opp.EnergySaved;
                summary.TotalCO2 += opp.CO2Saved;
            }

            summary.TreesEquivalent = summary.TotalCO2 / 20.0; // 1 tree absorbs ~20kg CO2/year
            
            return summary;
        }
    }

    // Data classes
    public class SavingsOpportunity
    {
        public string Icon { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public double MoneySaved { get; set; }
        public double EnergySaved { get; set; }
        public double CO2Saved { get; set; }
        public string Reason { get; set; } = "";
        public bool IsChecked { get; set; } = false;
    }

    public class DailySummary
    {
        public double TotalMoney { get; set; }
        public double TotalEnergy { get; set; }
        public double TotalCO2 { get; set; }
        public double TreesEquivalent { get; set; }
    }

    public class MonthlySummary
    {
        public double TotalMoney { get; set; }
        public double TotalEnergy { get; set; }
        public double TotalCO2 { get; set; }
        public int DaysActive { get; set; }
        public int CurrentStreak { get; set; }
        public double TreesEquivalent { get; set; }
    }
}