using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace WeatherApp
{
    public partial class MainForm : Form
    {
        private WeatherService _weatherService = new WeatherService();
        private Panel currentPage;
        private string currentCity = "London";
        private WeatherData currentWeather;
        
        public MainForm()
        {
            // Remove InitializeComponent call since we're building UI dynamically
            InitializeForm();
            InitializeNavigation();
            ShowDashboard();
            _ = LoadWeatherData(); // Fixed async call
        }

        private void InitializeForm()
        {
            this.Text = "🌤️ Weather Pro";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(25, 25, 25);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 10);
        }

        private void InitializeNavigation()
        {
            Panel navPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(15, 15, 15)
            };

            // Logo
            Label logo = new Label
            {
                Text = "🌤️ WEATHER PRO",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 150, 255),
                Location = new Point(20, 15),
                AutoSize = true
            };

            // Navigation buttons
            Button btnDashboard = CreateNavButton("📊 Dashboard", 250);
            Button btnMap = CreateNavButton("🗺️ Map", 400);
            Button btnSearch = CreateNavButton("🔍 Search", 550);

            btnDashboard.Click += (s, e) => ShowDashboard();
            btnMap.Click += (s, e) => ShowMap();
            btnSearch.Click += (s, e) => ShowSearch();

            // City selector
            ComboBox cityCombo = new ComboBox
            {
                Location = new Point(800, 15),
                Size = new Size(200, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White
            };
            cityCombo.Items.AddRange(new string[] { "London", "New York", "Tokyo", "Paris", "Sydney", "Dubai", "Mumbai", "São Paulo", "Cairo", "Moscow" });
            cityCombo.SelectedIndex = 0;
            cityCombo.SelectedIndexChanged += async (s, e) => 
            {
                currentCity = cityCombo.SelectedItem.ToString();
                await LoadWeatherData();
            };

            // Refresh button
            Button refreshBtn = new Button
            {
                Text = "🔄 Refresh",
                Location = new Point(1010, 15),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(0, 150, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            refreshBtn.Click += async (s, e) => await LoadWeatherData();

            navPanel.Controls.Add(logo);
            navPanel.Controls.Add(btnDashboard);
            navPanel.Controls.Add(btnMap);
            navPanel.Controls.Add(btnSearch);
            navPanel.Controls.Add(cityCombo);
            navPanel.Controls.Add(refreshBtn);
            
            this.Controls.Add(navPanel);
        }

        private Button CreateNavButton(string text, int x)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(x, 15),
                Size = new Size(120, 30),
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
            
            return btn;
        }

        private async Task LoadWeatherData()
        {
            try
            {
                currentWeather = await _weatherService.GetWeatherAsync(currentCity);
                UpdateDashboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Weather Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateDashboard()
        {
            if (currentPage != null && currentPage.Name == "dashboard" && currentWeather != null)
            {
                currentPage.Controls.Clear();
                CreateDashboardContent(currentPage);
            }
        }

        private void ShowDashboard()
        {
            ClearCurrentPage();
            currentPage = new Panel
            {
                Dock = DockStyle.Fill,
                Name = "dashboard",
                BackColor = Color.FromArgb(30, 30, 30)
            };
            this.Controls.Add(currentPage);
            
            if (currentWeather != null)
            {
                CreateDashboardContent(currentPage);
            }
            else
            {
                // Show loading message
                Label loadingLabel = new Label
                {
                    Text = "Loading weather data...",
                    Font = new Font("Segoe UI", 16),
                    ForeColor = Color.White,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                currentPage.Controls.Add(loadingLabel);
            }
            
            currentPage.BringToFront();
        }

        private void CreateDashboardContent(Panel panel)
        {
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 2,
                Padding = new Padding(20),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));

            // Main weather card
            Panel mainCard = CreateCard();
            mainCard.Controls.Add(CreateMainWeatherPanel());
            mainLayout.Controls.Add(mainCard, 0, 0);

            // Details card
            Panel detailsCard = CreateCard();
            detailsCard.Controls.Add(CreateDetailsPanel());
            mainLayout.Controls.Add(detailsCard, 1, 0);

            // Forecast card
            Panel forecastCard = CreateCard();
            forecastCard.Controls.Add(CreateForecastPanel());
            mainLayout.Controls.Add(forecastCard, 2, 0);

            // Hourly card
            Panel hourlyCard = CreateCard();
            hourlyCard.Controls.Add(CreateHourlyPanel());
            mainLayout.SetColumnSpan(hourlyCard, 2);
            mainLayout.Controls.Add(hourlyCard, 0, 1);

            // Additional info card
            Panel infoCard = CreateCard();
            infoCard.Controls.Add(CreateInfoPanel());
            mainLayout.Controls.Add(infoCard, 2, 1);

            panel.Controls.Add(mainLayout);
        }

        private Panel CreateCard()
        {
            return new Panel
            {
                BackColor = Color.FromArgb(40, 40, 40),
                Margin = new Padding(5),
                Padding = new Padding(15),
                Dock = DockStyle.Fill
            };
        }

        private Panel CreateMainWeatherPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill
            };

            // City and time
            Label cityLabel = new Label
            {
                Text = currentWeather.City,
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(0, 10),
                AutoSize = true
            };

            Label timeLabel = new Label
            {
                Text = DateTime.Now.ToString("dddd, MMMM dd • hh:mm tt"),
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Gray,
                Location = new Point(0, 50),
                AutoSize = true
            };

            // Temperature
            Label tempLabel = new Label
            {
                Text = $"{currentWeather.Temperature:0}°C",
                Font = new Font("Segoe UI", 48, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(0, 80),
                AutoSize = true
            };

            // Weather condition
            Label conditionLabel = new Label
            {
                Text = $"{GetWeatherIcon(currentWeather.WeatherIcon)} {currentWeather.WeatherDescription}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 150, 255),
                Location = new Point(0, 150),
                AutoSize = true
            };

            // Feels like
            Label feelsLabel = new Label
            {
                Text = $"Feels like {currentWeather.FeelsLike:0}°C",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.Gray,
                Location = new Point(0, 180),
                AutoSize = true
            };

            panel.Controls.Add(cityLabel);
            panel.Controls.Add(timeLabel);
            panel.Controls.Add(tempLabel);
            panel.Controls.Add(conditionLabel);
            panel.Controls.Add(feelsLabel);

            return panel;
        }

        private Panel CreateDetailsPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill
            };

            Label title = new Label
            {
                Text = "Details",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(0, 0),
                AutoSize = true
            };

            int y = 40;
            AddDetailRow(panel, "💧 Humidity", $"{currentWeather.Humidity}%", y);
            AddDetailRow(panel, "💨 Wind", $"{currentWeather.WindSpeed:0.0} m/s", y + 40);
            AddDetailRow(panel, "🌫️ Pressure", $"{currentWeather.Pressure} hPa", y + 80);
            AddDetailRow(panel, "👁️ Visibility", $"{currentWeather.Visibility / 1000:0.0} km", y + 120);
            AddDetailRow(panel, "☁️ Cloudiness", $"{currentWeather.Cloudiness}%", y + 160);

            return panel;
        }

        private void AddDetailRow(Panel panel, string label, string value, int y)
        {
            Label lbl = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.Gray,
                Location = new Point(0, y),
                Size = new Size(150, 30)
            };

            Label val = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(150, y),
                Size = new Size(100, 30),
                TextAlign = ContentAlignment.MiddleRight
            };

            panel.Controls.Add(lbl);
            panel.Controls.Add(val);
        }

        private Panel CreateForecastPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill
            };

            Label title = new Label
            {
                Text = "5-Day Forecast",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(0, 0),
                AutoSize = true
            };

            // Simple forecast items
            string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri" };
            string[] icons = { "☀️", "⛅", "🌧️", "☀️", "☁️" };
            string[] temps = { "22°/18°", "20°/16°", "18°/14°", "21°/17°", "19°/15°" };

            for (int i = 0; i < 5; i++)
            {
                Panel dayPanel = new Panel
                {
                    Location = new Point(0, 40 + i * 40),
                    Size = new Size(200, 35)
                };

                Label dayLabel = new Label
                {
                    Text = days[i],
                    Font = new Font("Segoe UI", 12),
                    ForeColor = Color.Gray,
                    Location = new Point(0, 5),
                    Size = new Size(50, 25)
                };

                Label iconLabel = new Label
                {
                    Text = icons[i],
                    Font = new Font("Segoe UI", 14),
                    Location = new Point(60, 5),
                    Size = new Size(30, 25)
                };

                Label tempLabel = new Label
                {
                    Text = temps[i],
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(100, 5),
                    Size = new Size(100, 25),
                    TextAlign = ContentAlignment.MiddleRight
                };

                dayPanel.Controls.Add(dayLabel);
                dayPanel.Controls.Add(iconLabel);
                dayPanel.Controls.Add(tempLabel);
                panel.Controls.Add(dayPanel);
            }

            return panel;
        }

        private Panel CreateHourlyPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill
            };

            Label title = new Label
            {
                Text = "Hourly Forecast",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(0, 0),
                AutoSize = true
            };

            FlowLayoutPanel hourlyFlow = new FlowLayoutPanel
            {
                Location = new Point(0, 40),
                Size = new Size(600, 100),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoScroll = true
            };

            for (int i = 0; i < 24; i += 2)
            {
                Panel hourCard = new Panel
                {
                    Size = new Size(80, 90),
                    BackColor = Color.FromArgb(50, 50, 50),
                    Margin = new Padding(5),
                    Padding = new Padding(10)
                };

                Label timeLabel = new Label
                {
                    Text = $"{i}:00",
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.Gray,
                    Location = new Point(15, 5),
                    AutoSize = true
                };

                Label tempLabel = new Label
                {
                    Text = $"{new Random().Next(15, 25)}°",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(25, 35),
                    AutoSize = true
                };

                hourCard.Controls.Add(timeLabel);
                hourCard.Controls.Add(tempLabel);
                hourlyFlow.Controls.Add(hourCard);
            }

            panel.Controls.Add(title);
            panel.Controls.Add(hourlyFlow);

            return panel;
        }

        private Panel CreateInfoPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill
            };

            Label title = new Label
            {
                Text = "Additional Info",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(0, 0),
                AutoSize = true
            };

            // UV Index
            Panel uvPanel = new Panel
            {
                Location = new Point(0, 40),
                Size = new Size(200, 60),
                BackColor = Color.FromArgb(0, 100, 0),
                Padding = new Padding(10)
            };

            Label uvLabel = new Label
            {
                Text = "UV Index: 5\nModerate",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill
            };

            // Sunrise/Sunset
            string sunriseTime = currentWeather.Sunrise?.ToString("hh:mm tt") ?? "6:45 AM";
            string sunsetTime = currentWeather.Sunset?.ToString("hh:mm tt") ?? "7:30 PM";
            
            Panel sunPanel = new Panel
            {
                Location = new Point(0, 110),
                Size = new Size(200, 60),
                BackColor = Color.FromArgb(50, 50, 80),
                Padding = new Padding(10)
            };

            Label sunLabel = new Label
            {
                Text = $"Sunrise: {sunriseTime}\nSunset: {sunsetTime}",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White,
                Dock = DockStyle.Fill
            };

            uvPanel.Controls.Add(uvLabel);
            sunPanel.Controls.Add(sunLabel);

            panel.Controls.Add(title);
            panel.Controls.Add(uvPanel);
            panel.Controls.Add(sunPanel);

            return panel;
        }

        private string GetWeatherIcon(string iconCode)
        {
            if (string.IsNullOrEmpty(iconCode)) return "🌤️";
            
            return iconCode switch
            {
                "01d" => "☀️",
                "01n" => "🌙",
                "02d" => "⛅",
                "02n" => "☁️",
                "03d" or "03n" => "☁️",
                "04d" or "04n" => "☁️",
                "09d" or "09n" => "🌧️",
                "10d" or "10n" => "🌦️",
                "11d" or "11n" => "⛈️",
                "13d" or "13n" => "❄️",
                "50d" or "50n" => "🌫️",
                _ => "🌤️"
            };
        }

        private void ShowMap()
        {
            ClearCurrentPage();
            currentPage = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 30),
                Padding = new Padding(40)
            };

            Label title = new Label
            {
                Text = "Weather Map",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 150, 255),
                Location = new Point(0, 0),
                AutoSize = true
            };

            Label description = new Label
            {
                Text = "Interactive weather map with real-time data\n\n" +
                       "Features:\n" +
                       "• Live temperature overlay\n" +
                       "• Precipitation radar\n" +
                       "• Wind direction visualization\n" +
                       "• Storm tracking\n\n" +
                       "Map integration ready for OpenStreetMap or Google Maps",
                Font = new Font("Segoe UI", 14),
                ForeColor = Color.White,
                Location = new Point(0, 80),
                Size = new Size(1000, 300)
            };

            Panel mapPlaceholder = new Panel
            {
                Location = new Point(0, 300),
                Size = new Size(1000, 300),
                BackColor = Color.FromArgb(20, 20, 40),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label mapLabel = new Label
            {
                Text = "🌍 Map View\n(Integrate with mapping library)",
                Font = new Font("Segoe UI", 20),
                ForeColor = Color.Gray,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            mapPlaceholder.Controls.Add(mapLabel);
            currentPage.Controls.Add(title);
            currentPage.Controls.Add(description);
            currentPage.Controls.Add(mapPlaceholder);
            this.Controls.Add(currentPage);
            currentPage.BringToFront();
        }

        private void ShowSearch()
        {
            ClearCurrentPage();
            currentPage = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 30),
                Padding = new Padding(40)
            };

            Label title = new Label
            {
                Text = "Search Cities",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 150, 255),
                Location = new Point(0, 0),
                AutoSize = true
            };

            // Search box
            Panel searchBox = new Panel
            {
                Location = new Point(0, 80),
                Size = new Size(800, 60),
                BackColor = Color.FromArgb(50, 50, 50)
            };

            TextBox searchInput = new TextBox
            {
                Location = new Point(20, 15),
                Size = new Size(600, 30),
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 12),
                PlaceholderText = "Enter city name..."
            };

            Button searchBtn = new Button
            {
                Text = "🔍 Search",
                Location = new Point(630, 10),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(0, 150, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            searchBox.Controls.Add(searchInput);
            searchBox.Controls.Add(searchBtn);

            // Popular cities
            Label popularLabel = new Label
            {
                Text = "Popular Cities:",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(0, 160),
                AutoSize = true
            };

            FlowLayoutPanel citiesPanel = new FlowLayoutPanel
            {
                Location = new Point(0, 200),
                Size = new Size(1000, 100),
                FlowDirection = FlowDirection.LeftToRight
            };

            string[] cities = { "London", "New York", "Tokyo", "Paris", "Sydney", "Dubai", "Mumbai", "São Paulo", "Cairo", "Moscow" };
            foreach (var city in cities)
            {
                Button cityBtn = new Button
                {
                    Text = city,
                    Size = new Size(120, 40),
                    BackColor = Color.FromArgb(60, 60, 60),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 11),
                    Tag = city
                };
                
                cityBtn.Click += async (s, e) =>
                {
                    currentCity = cityBtn.Tag.ToString();
                    await LoadWeatherData();
                    ShowDashboard();
                };
                
                citiesPanel.Controls.Add(cityBtn);
            }

            searchBtn.Click += async (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(searchInput.Text))
                {
                    currentCity = searchInput.Text;
                    await LoadWeatherData();
                    ShowDashboard();
                }
            };

            currentPage.Controls.Add(title);
            currentPage.Controls.Add(searchBox);
            currentPage.Controls.Add(popularLabel);
            currentPage.Controls.Add(citiesPanel);
            this.Controls.Add(currentPage);
            currentPage.BringToFront();
        }

        private void ClearCurrentPage()
        {
            if (currentPage != null)
            {
                this.Controls.Remove(currentPage);
                currentPage.Dispose();
                currentPage = null;
            }
        }
    }
}