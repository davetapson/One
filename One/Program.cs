using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace One
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainGUI());

            /*
             * 1. Poll for file from given directory
             * 2. Load file into Trades table
             * 3. Create Orders from Trades table, save to PlaceOrders table, status 'unprocessed'
             * 4. Extract from PlaceOrders table, create and send orders - status processed
             * 5. Catch Executions, save to Executions table
             * 6. Catch Open Orders, save in Orders table
             * 7. Catch OrderStatuses, save in OrderStatus's table.
             * 8. (Update Orders table?)
             * 9. Reports to watch over all
             * */
        }
    }
}
