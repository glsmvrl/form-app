namespace FirstFormApp
{
    public partial class Form1 : Form
    {
        string boatTourSelect = "";
        int boatWidth;
        DateTime startDate;
        DateTime finishDate;
        bool calculateActive = false;
        int price = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            boatTourSelect = "Motor Yat";

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            boatTourSelect = "Gulet";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            boatWidth = 14;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            boatWidth = 15;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            boatWidth = 16;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            startDate = dateTimePicker1.Value;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.MinDate = startDate;
            finishDate = dateTimePicker2.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            calculateActive = true;
            System.Threading.Thread.Sleep(700);
            Form2 f2 = new Form2(this);
            f2.ShowDialog();
        }

        public string CalculateTotalPrice()
        {
            if (calculateActive == true)
            {
                DateCalculator calculator = new DateCalculator();
                Dictionary<int, int> resultCalculateDaysPerMonth = calculator.CalculateDaysPerMonth(startDate, finishDate);

                foreach (var item in resultCalculateDaysPerMonth)
                {
                    Console.WriteLine($"{item.Key}. ayda : {item.Value} gün");
                }

                int totalPrice = calculator.CalculatePerMonthPrice(boatTourSelect, boatWidth, resultCalculateDaysPerMonth);
                price = totalPrice;

            }

            if(boatTourSelect == "Gulet")
            {
                return $"{price} €";

            } else
            {
                return $"{price} ₺";
            }

        }
    }

    public class DateCalculator
    {
        public int CalculateTotalDays(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).Days + 1;
        }

        public int CalculatePerMonthPrice(string boatTourInput, int boatWidth, Dictionary<int, int> daysPerMonth)
        {

            int[][] guletPrices = new int[3][]
            {
                  new int[] { 400, 450, 500, 550, 550, 500, 450 },
                  new int[] { 500, 700, 750, 850, 900, 750, 700 },
                  new int[] { 250, 400, 550, 650, 650, 550, 400 },
            };

            int[][] motorYachtPrices = new int[3][]
            {
                  new int[] { 600 , 750 ,900 ,1200 ,1200, 900, 750 },
                  new int[] { 900 ,1000,1100 ,1200 ,1200, 1100, 1000 },
                  new int[] { 1250, 1500, 1750, 2000, 2000, 1750, 1500 },
            };

            Dictionary<int, int[]> guletPricesBtMonths = new Dictionary<int, int[]>();
            guletPricesBtMonths.Add(16, guletPrices[0]);
            guletPricesBtMonths.Add(15, guletPrices[1]);
            guletPricesBtMonths.Add(14, guletPrices[2]);

            Dictionary<int, int[]> motorYatchPricesByMonths = new Dictionary<int, int[]>();
            motorYatchPricesByMonths.Add(14, motorYachtPrices[0]);
            motorYatchPricesByMonths.Add(15, motorYachtPrices[1]);
            motorYatchPricesByMonths.Add(16, motorYachtPrices[2]);

            int secilenPrice = 0;
            int totalPrice = 0;

            foreach (var item in daysPerMonth)
            {

                if (boatTourInput == "Gulet")
                {
                    secilenPrice = guletPricesBtMonths[boatWidth][item.Key - 4];
                    totalPrice += secilenPrice * item.Value;
                }
                else
                {
                    secilenPrice = motorYatchPricesByMonths[boatWidth][item.Key - 4];
                    totalPrice += secilenPrice * item.Value;
                }
            }

            if (boatTourInput == "Gulet")
            {
                Console.WriteLine($"Toplam Fiyat: {totalPrice}£");
            }
            else
            {
                Console.WriteLine($"Toplam Fiyat: {totalPrice}₺");

            }

            return (totalPrice);
        }

        public Dictionary<int, int> CalculateDaysPerMonth(DateTime startDate, DateTime endDate)
        {
            Dictionary<int, int> daysPerMonth = new Dictionary<int, int>();
            DateTime current = startDate;

            while (current <= endDate)
            {
                int monthKeyInt = current.Month;
                if (!daysPerMonth.ContainsKey(monthKeyInt))
                {
                    daysPerMonth[monthKeyInt] = 0;
                }

                DateTime nextMonth = new DateTime(current.Year, current.Month, 1).AddMonths(1);
                DateTime periodEnd = (nextMonth <= endDate) ? nextMonth.AddDays(-1) : endDate;

                int daysInCurrentMonth = (periodEnd - current).Days + 1;

                daysPerMonth[monthKeyInt] += daysInCurrentMonth;

                current = nextMonth;
            }


            return daysPerMonth;
        }
    }
}
