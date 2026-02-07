using System;
using System.Collections.Generic;
using System.Linq;

namespace WeatherApp
{
    public class AdvisorService
    {
        private static Random random = new Random();

        // Get clothing suggestions based on weather
        public static ClothingAdvice GetClothingAdvice(WeatherData weather)
        {
            var advice = new ClothingAdvice();
            double temp = weather.Temperature;
            string condition = weather.WeatherCondition?.ToLower() ?? "";
            double windSpeed = weather.WindSpeed;
            int humidity = weather.Humidity;

            // Temperature-based clothing
            if (temp < -10)
            {
                advice.MainClothing = "Heavy winter coat, thermal layers, insulated pants";
                advice.Accessories = "Thick gloves, warm hat, scarf, winter boots";
                advice.ClothingEmojis = new[] { "🧥", "🧤", "🎩", "🧣" };
                advice.Description = "Extreme cold - Multiple layers essential!";
            }
            else if (temp < 0)
            {
                advice.MainClothing = "Winter coat, warm sweater, jeans or warm pants";
                advice.Accessories = "Gloves, beanie, scarf, winter shoes";
                advice.ClothingEmojis = new[] { "🧥", "🧤", "🎩", "🧣" };
                advice.Description = "Freezing temperatures - Bundle up!";
            }
            else if (temp < 10)
            {
                advice.MainClothing = "Jacket or coat, long-sleeve shirt, pants";
                advice.Accessories = "Light gloves, hat optional, closed shoes";
                advice.ClothingEmojis = new[] { "🧥", "👖", "👟" };
                advice.Description = "Cold weather - Warm layers needed";
            }
            else if (temp < 15)
            {
                advice.MainClothing = "Light jacket, long-sleeve shirt, pants or jeans";
                advice.Accessories = "Scarf optional, comfortable shoes";
                advice.ClothingEmojis = new[] { "🧥", "👕", "👖", "👟" };
                advice.Description = "Cool weather - Light layers recommended";
            }
            else if (temp < 20)
            {
                advice.MainClothing = "Long-sleeve shirt or light sweater, pants";
                advice.Accessories = "Sunglasses, comfortable shoes";
                advice.ClothingEmojis = new[] { "👕", "👖", "👟", "🕶️" };
                advice.Description = "Mild weather - Comfortable clothing";
            }
            else if (temp < 25)
            {
                advice.MainClothing = "T-shirt or short-sleeve shirt, pants or shorts";
                advice.Accessories = "Sunglasses, light shoes or sneakers";
                advice.ClothingEmojis = new[] { "👕", "🩳", "👟", "🕶️" };
                advice.Description = "Pleasant weather - Light clothing";
            }
            else if (temp < 30)
            {
                advice.MainClothing = "Light t-shirt, shorts or light pants";
                advice.Accessories = "Sunglasses, sandals or light shoes, sun hat";
                advice.ClothingEmojis = new[] { "👕", "🩳", "🩴", "🕶️", "🧢" };
                advice.Description = "Warm weather - Stay cool and light";
            }
            else
            {
                advice.MainClothing = "Very light, breathable clothing, shorts";
                advice.Accessories = "Sunglasses, sandals, wide-brimmed hat, sunscreen";
                advice.ClothingEmojis = new[] { "👕", "🩳", "🩴", "🕶️", "🧢" };
                advice.Description = "Hot weather - Minimal, breathable clothing";
            }

            // Weather condition adjustments
            if (condition.Contains("rain") || condition.Contains("drizzle"))
            {
                advice.ExtraItems = "☔ Umbrella or raincoat essential!";
                advice.AdditionalTip = "Waterproof shoes recommended";
            }
            else if (condition.Contains("snow"))
            {
                advice.ExtraItems = "❄️ Waterproof boots and warm socks";
                advice.AdditionalTip = "Extra layer for snow activities";
            }
            else if (condition.Contains("thunderstorm"))
            {
                advice.ExtraItems = "⛈️ Waterproof gear and avoid metal accessories";
                advice.AdditionalTip = "Stay indoors if possible";
            }
            else if (windSpeed > 10)
            {
                advice.ExtraItems = "🌬️ Windbreaker or wind-resistant jacket";
                advice.AdditionalTip = "Secure loose items";
            }

            // UV protection for sunny days
            if (condition.Contains("clear") || condition.Contains("sun"))
            {
                advice.UVProtection = "☀️ Apply sunscreen (SPF 30+), wear UV-protective sunglasses";
            }

            return advice;
        }

        // Get health advice based on weather
        public static HealthAdvice GetHealthAdvice(WeatherData weather)
        {
            var advice = new HealthAdvice();
            double temp = weather.Temperature;
            int humidity = weather.Humidity;
            string condition = weather.WeatherCondition?.ToLower() ?? "";

            advice.Tips = new List<string>();
            advice.Warnings = new List<string>();

            // Temperature-based health advice
            if (temp < 0)
            {
                advice.MainAdvice = "⚠️ Risk of Hypothermia and Frostbite";
                advice.Tips.Add("💧 Stay hydrated - you lose moisture in cold air");
                advice.Tips.Add("🏃 Limit outdoor exposure to 30 minutes at a time");
                advice.Tips.Add("❤️ Watch for signs of hypothermia: shivering, confusion, drowsiness");
                advice.Warnings.Add("Frostbite can occur in minutes on exposed skin");
                advice.Warnings.Add("Breathing cold air can trigger asthma attacks");
            }
            else if (temp < 10)
            {
                advice.MainAdvice = "🌡️ Cold Weather Precautions";
                advice.Tips.Add("🫁 Breathe through your nose to warm the air");
                advice.Tips.Add("💪 Warm up before outdoor exercise");
                advice.Tips.Add("🍲 Eat warm, nutritious meals to maintain body heat");
                advice.Warnings.Add("Cold weather can worsen joint pain");
            }
            else if (temp < 15)
            {
                advice.MainAdvice = "😊 Comfortable Temperature Range";
                advice.Tips.Add("🚶 Great weather for outdoor activities");
                advice.Tips.Add("💧 Maintain regular hydration");
                advice.Tips.Add("🧘 Perfect for exercise and fresh air");
            }
            else if (temp < 25)
            {
                advice.MainAdvice = "☀️ Ideal Weather Conditions";
                advice.Tips.Add("🚴 Excellent for outdoor exercise");
                advice.Tips.Add("🌳 Spend time in nature for mental health");
                advice.Tips.Add("💧 Drink water regularly");
            }
            else if (temp < 30)
            {
                advice.MainAdvice = "🌡️ Warm Weather - Stay Cool";
                advice.Tips.Add("💧 Increase water intake - drink before you're thirsty");
                advice.Tips.Add("⏰ Avoid strenuous activity during peak hours (12-3pm)");
                advice.Tips.Add("🍉 Eat light, hydrating foods like fruits");
                advice.Tips.Add("☀️ Wear sunscreen and reapply every 2 hours");
                advice.Warnings.Add("Risk of dehydration and heat exhaustion");
            }
            else
            {
                advice.MainAdvice = "🚨 Heat Warning - High Risk";
                advice.Tips.Add("💧 Drink water every 15-20 minutes, even if not thirsty");
                advice.Tips.Add("🏠 Stay indoors in air-conditioned spaces when possible");
                advice.Tips.Add("🌡️ Check on elderly neighbors and pets");
                advice.Tips.Add("🚫 Avoid alcohol and caffeine - they dehydrate you");
                advice.Tips.Add("🧊 Take cool showers or baths");
                advice.Warnings.Add("⚠️ HIGH RISK of heat stroke and heat exhaustion");
                advice.Warnings.Add("Symptoms: Heavy sweating, weakness, dizziness, nausea");
                advice.Warnings.Add("Seek immediate medical help if symptoms occur");
            }

            // Humidity-based advice
            if (humidity > 80)
            {
                advice.Tips.Add("💨 High humidity makes it feel hotter - limit outdoor time");
                advice.Tips.Add("🏃 Your body can't cool down as efficiently through sweating");
            }
            else if (humidity < 30)
            {
                advice.Tips.Add("💧 Low humidity can dry out skin and airways");
                advice.Tips.Add("🧴 Use moisturizer and lip balm");
                advice.Tips.Add("💨 Use a humidifier indoors if possible");
            }

            // Weather condition specific advice
            if (condition.Contains("rain"))
            {
                advice.Tips.Add("🦠 Wash hands frequently - wet weather spreads germs");
                advice.Tips.Add("🌈 Don't let rain stop you - light rain can be refreshing!");
            }
            else if (condition.Contains("snow"))
            {
                advice.Tips.Add("⚠️ Watch for icy surfaces - risk of falls and injuries");
                advice.Tips.Add("👀 Snow glare can damage eyes - wear sunglasses");
            }
            else if (condition.Contains("thunderstorm"))
            {
                advice.Warnings.Add("⚡ Stay indoors during thunderstorms");
                advice.Warnings.Add("Avoid using corded phones and electrical appliances");
            }

            // Air quality note
            if (weather.Visibility < 5000)
            {
                advice.Tips.Add("😷 Poor visibility may indicate air pollution - limit outdoor activity");
            }

            return advice;
        }

        // Get random fun weather fact
        public static string GetRandomWeatherFact(WeatherData weather)
        {
            var facts = new List<string>();
            
            // General weather facts
            facts.AddRange(new[]
            {
                "Did you know? The common cold is caused by viruses, NOT by cold weather! However, cold weather can weaken your immune system, making you more susceptible to viruses.",
                "Did you know? Lightning strikes the Earth about 100 times every second! That's over 8 million strikes per day.",
                "Did you know? Snowflakes are actually translucent, not white! They appear white because light bounces off the many surfaces of the ice crystal.",
                "Did you know? The highest temperature ever recorded on Earth was 134°F (56.7°C) in Death Valley, California in 1913!",
                "Did you know? The lowest temperature ever recorded was -128.6°F (-89.2°C) in Antarctica in 1983!",
                "Did you know? A raindrop falls at approximately 20 mph (32 km/h) - fast enough to hurt if it were solid!",
                "Did you know? Hurricanes, typhoons, and cyclones are all the same weather phenomenon - they're just called different names in different parts of the world!",
                "Did you know? Humidity makes hot weather feel hotter because it prevents sweat from evaporating, which is your body's main cooling mechanism!",
                "Did you know? Wind chill can make the temperature feel much colder than it actually is! At -20°C with 50 km/h wind, it can feel like -33°C!",
                "Did you know? Your body loses about 2-3 liters of water per day through breathing, sweating, and other bodily functions - more in hot weather!",
                "Did you know? Frostbite can occur in as little as 5-10 minutes when skin is exposed to temperatures below -28°C!",
                "Did you know? The safest place during a thunderstorm is inside a building or car, NOT under a tree!",
            });

            // Temperature-specific facts
            double temp = weather.Temperature;
            if (temp < 0)
            {
                facts.AddRange(new[]
                {
                    "Did you know? Hot beverages don't actually warm you up! They make you sweat, which cools you down. Lukewarm drinks are better in the cold!",
                    "Did you know? Your body burns more calories in cold weather trying to maintain its core temperature!",
                    "Did you know? Exposed skin can freeze in under 30 minutes at temperatures below -18°C (0°F)!",
                });
            }
            else if (temp > 30)
            {
                facts.AddRange(new[]
                {
                    "Did you know? Heat exhaustion can occur when your body temperature reaches 104°F (40°C)!",
                    "Did you know? Drinking ice-cold water in hot weather can cause stomach cramps. Room temperature water is better absorbed!",
                    "Did you know? Light-colored, loose-fitting clothes reflect heat better than dark, tight clothes!",
                });
            }

            // Weather condition specific facts
            string condition = weather.WeatherCondition?.ToLower() ?? "";
            if (condition.Contains("rain"))
            {
                facts.AddRange(new[]
                {
                    "Did you know? 'Petrichor' is the name for the pleasant smell after rain - it's caused by oils released from plants and bacteria!",
                    "Did you know? A single thunderstorm cloud can hold up to 275 million gallons of water!",
                    "Did you know? Raindrops are shaped like hamburger buns, not teardrops!",
                });
            }
            else if (condition.Contains("snow"))
            {
                facts.AddRange(new[]
                {
                    "Did you know? No two snowflakes are exactly alike! Each has a unique crystalline structure.",
                    "Did you know? Snow isn't actually frozen rain - it forms when water vapor crystallizes directly from gas to solid!",
                    "Did you know? Snow can actually keep you warm! Igloos work because snow is an excellent insulator.",
                });
            }
            else if (condition.Contains("clear") || condition.Contains("sun"))
            {
                facts.AddRange(new[]
                {
                    "Did you know? Just 15-20 minutes of sunlight exposure helps your body produce vitamin D!",
                    "Did you know? UV rays can penetrate clouds, so you can get sunburned even on overcast days!",
                    "Did you know? The sun's UV rays are strongest between 10 AM and 4 PM!",
                });
            }

            // Return random fact
            return facts[random.Next(facts.Count)];
        }
    }

    // Data classes for advisor responses
    public class ClothingAdvice
    {
        public string Description { get; set; } = "";
        public string MainClothing { get; set; } = "";
        public string Accessories { get; set; } = "";
        public string[] ClothingEmojis { get; set; } = Array.Empty<string>();
        public string ExtraItems { get; set; } = "";
        public string AdditionalTip { get; set; } = "";
        public string UVProtection { get; set; } = "";
    }

    public class HealthAdvice
    {
        public string MainAdvice { get; set; } = "";
        public List<string> Tips { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }
}