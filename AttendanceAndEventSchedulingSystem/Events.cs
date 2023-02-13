using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AttendanceAndEventSchedulingSystem
{

    public partial class Events : Form
    {
        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=aess_db");
        MySqlCommand command;
        string addEventString = "INSERT INTO aess_db.events (`name`,`description`,`location`,`schedule`,`start_hour`,`end_hour`,`event_status`) " +
            "VALUES (@name, @description, @location, @schedule, @start_hour, @end_hour, @event_status)";

        string retrieveEventsDataString = "SELECT id, name, description, location, " +
            "schedule, start_hour, end_hour, event_status, created_at FROM " +
            "events WHERE status = 'a'";

        string updateEventQueryString = "UPDATE `events` SET `name`=@name,`description`=@description," +
            "`location`=@location,`schedule`=@schedule,`start_hour`=@start_hour,`end_hour`=@end_hour,`event_status`=@event_status WHERE `id` = @Id";

        string deleteEventQueryString = "DELETE FROM `events` WHERE `id` = @Id";

        int selectedRowIndex;

        public Events()
        {
            InitializeComponent();
            Display_EventList();
        }
        private void ClearData()
        {
            txtEventName.Text = "";
            txtDescription.Text = "";
            txtLocation.Text = "";
            cbStatus.Text = "";
            dtpSchedule.Value = DateTime.Today;
        }
        public void Display_EventList() {
            MySqlDataAdapter adapter;
            DataTable table;
            try
            {
                connection.Open();
                command = new MySqlCommand(retrieveEventsDataString, connection);
                adapter = new MySqlDataAdapter(command);
                table = new DataTable();
                adapter.Fill(table);

                dgvEventList.DataSource = null;
                dgvEventList.AutoGenerateColumns = false;

                dgvEventList.ColumnCount = 9;

                dgvEventList.Columns[0].Name = "id";
                dgvEventList.Columns[0].HeaderText = "Id";
                dgvEventList.Columns[0].DataPropertyName = "id";
                dgvEventList.Columns[0].Width = 20;

                dgvEventList.Columns[1].Name = "name";
                dgvEventList.Columns[1].HeaderText = "Event Name";
                dgvEventList.Columns[1].DataPropertyName = "name";
                dgvEventList.Columns[1].Width = 100;

                dgvEventList.Columns[2].Name = "description";
                dgvEventList.Columns[2].HeaderText = "Description";
                dgvEventList.Columns[2].DataPropertyName = "description";
                dgvEventList.Columns[2].Width = 95;

                dgvEventList.Columns[3].Name = "location";
                dgvEventList.Columns[3].HeaderText = "Location";
                dgvEventList.Columns[3].DataPropertyName = "location";
                dgvEventList.Columns[3].Width = 80;

                dgvEventList.Columns[4].Name = "schedule";
                dgvEventList.Columns[4].HeaderText = "Schedule";
                dgvEventList.Columns[4].DataPropertyName = "schedule";
                dgvEventList.Columns[4].Width = 70;

                dgvEventList.Columns[5].Name = "start_hour";
                dgvEventList.Columns[5].HeaderText = "Start";
                dgvEventList.Columns[5].DataPropertyName = "start_hour";
                dgvEventList.Columns[5].Width = 50;

                dgvEventList.Columns[6].Name = "end_hour";
                dgvEventList.Columns[6].HeaderText = "End";
                dgvEventList.Columns[6].DataPropertyName = "end_hour";
                dgvEventList.Columns[6].Width = 50;

                dgvEventList.Columns[7].Name = "event_status";
                dgvEventList.Columns[7].HeaderText = "Status";
                dgvEventList.Columns[7].DataPropertyName = "event_status";
                dgvEventList.Columns[7].Width = 50;

                dgvEventList.Columns[8].Name = "created_at";
                dgvEventList.Columns[8].HeaderText = "Created Date";
                dgvEventList.Columns[8].DataPropertyName = "created_at";
                dgvEventList.Columns[8].Width = 110;

                dgvEventList.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                command.Dispose();
                connection.Close();
            }
        }

        private void btnAddEvent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEventName.Text) || string.IsNullOrEmpty(txtDescription.Text) || string.IsNullOrEmpty(txtLocation.Text) || string.IsNullOrEmpty(dtpSchedule.Text) || string.IsNullOrEmpty(dtpStartHour.Text) || string.IsNullOrEmpty(dtpEndHour.Text)) {
                if (string.IsNullOrEmpty(txtEventName.Text))
                {
                    MessageBox.Show("Event Name is required!");
                } 
                else if (string.IsNullOrEmpty(txtDescription.Text))
                {
                    MessageBox.Show("Description is required!");
                } 
                else if (string.IsNullOrEmpty(txtLocation.Text))
                {
                    MessageBox.Show("Location is required!");
                } 
                else if (string.IsNullOrEmpty(dtpSchedule.Text))
                {
                    MessageBox.Show("Schedule is required!");
                }
                else if (string.IsNullOrEmpty(dtpStartHour.Text))
                {
                    MessageBox.Show("Start hour is required!");
                }
                else if (string.IsNullOrEmpty(dtpEndHour.Text))
                {
                    MessageBox.Show("End hour is required!");
                }
                else
                {
                    MessageBox.Show("Please complete all fields!");
                }
            }
            else
            {
                try
                {
                    connection.Open();
                    command = new MySqlCommand(addEventString, connection);
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = txtEventName.Text;
                    command.Parameters.Add("@description", MySqlDbType.VarChar).Value = txtDescription.Text;
                    command.Parameters.Add("@location", MySqlDbType.VarChar).Value = txtLocation.Text;
                    command.Parameters.Add("@schedule", MySqlDbType.VarChar).Value = dtpSchedule.Text;
                    command.Parameters.Add("@start_hour", MySqlDbType.VarChar).Value = dtpStartHour.Text;
                    command.Parameters.Add("@end_hour", MySqlDbType.VarChar).Value = dtpEndHour.Text;
                    command.Parameters.Add("@event_status", MySqlDbType.VarChar).Value = "Ready";

                    int result = command.ExecuteNonQuery();

                    if (result == 1)
                    {
                        MessageBox.Show("Data added successfully!");
                        txtEventName.Text = "";
                        txtDescription.Text = "";
                        txtLocation.Text = "";
                        dtpSchedule.Text = "";
                        dtpStartHour.Text = "";
                        dtpEndHour.Text = "";
                        btnUpdateEvent.Enabled = false;
                        btnDeleteEvent.Enabled = false;
                        btnViewEvent.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Failed!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    Display_EventList();
                }
            }
        }

        private void dgvEventList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvEventList.SelectedRows.Count > 0) {
                selectedRowIndex = e.RowIndex;

                DataGridViewRow selectedDGVRow = dgvEventList.Rows[selectedRowIndex];
                txtId.Text = selectedDGVRow.Cells[0].Value.ToString();
                txtEventName.Text = selectedDGVRow.Cells[1].Value.ToString();
                txtDescription.Text = selectedDGVRow.Cells[2].Value.ToString();
                txtLocation.Text = selectedDGVRow.Cells[3].Value.ToString();    
                dtpSchedule.Text = selectedDGVRow.Cells[4].Value.ToString();
                dtpStartHour.Text = selectedDGVRow.Cells[5].Value.ToString();
                dtpEndHour.Text = selectedDGVRow.Cells[6].Value.ToString();
                cbStatus.Text = selectedDGVRow.Cells[7].Value.ToString();
                cbStatus.Enabled = true;
                btnUpdateEvent.Enabled = true;
                btnDeleteEvent.Enabled = true;
                btnAddEvent.Enabled = false;
            }
        }

        private void btnUpdateEvent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEventName.Text) || string.IsNullOrEmpty(txtDescription.Text) || string.IsNullOrEmpty(txtLocation.Text) || string.IsNullOrEmpty(dtpSchedule.Text) || string.IsNullOrEmpty(dtpStartHour.Text) || string.IsNullOrEmpty(dtpEndHour.Text) || string.IsNullOrEmpty(cbStatus.Text))
            {
                if (string.IsNullOrEmpty(txtEventName.Text))
                {
                    MessageBox.Show("Event Name is required!");
                }
                else if (string.IsNullOrEmpty(txtDescription.Text))
                {
                    MessageBox.Show("Description is required!");
                }
                else if (string.IsNullOrEmpty(txtLocation.Text))
                {
                    MessageBox.Show("Location is required!");
                }
                else if (string.IsNullOrEmpty(dtpSchedule.Text))
                {
                    MessageBox.Show("Schedule is required!");
                }
                else if (string.IsNullOrEmpty(dtpStartHour.Text))
                {
                    MessageBox.Show("Start hour is required!");
                }
                else if (string.IsNullOrEmpty(dtpEndHour.Text))
                {
                    MessageBox.Show("End hour is required!");
                }
                else if (string.IsNullOrEmpty(cbStatus.Text))
                {
                    MessageBox.Show("Status is required!");
                }
                else
                {
                    MessageBox.Show("Please complete all fields!");
                }
            }
            else
            {
                try
                {
                    connection.Open();
                    command = new MySqlCommand(updateEventQueryString, connection);
                    command.Parameters.Add("@Id", MySqlDbType.Int32).Value = int.Parse(txtId.Text);
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = txtEventName.Text;
                    command.Parameters.Add("@description", MySqlDbType.VarChar).Value = txtDescription.Text;
                    command.Parameters.Add("@location", MySqlDbType.VarChar).Value = txtLocation.Text;
                    command.Parameters.Add("@schedule", MySqlDbType.VarChar).Value = dtpSchedule.Text;
                    command.Parameters.Add("@start_hour", MySqlDbType.VarChar).Value = dtpStartHour.Text;
                    command.Parameters.Add("@end_hour", MySqlDbType.VarChar).Value = dtpEndHour.Text;
                    command.Parameters.Add("@event_status", MySqlDbType.VarChar).Value = cbStatus.Text;

                    int result = command.ExecuteNonQuery();

                    if (result == 1)
                    {
                        MessageBox.Show("Data updated successfully!");
                        txtEventName.Text = "";
                        txtDescription.Text = "";
                        txtLocation.Text = "";
                        dtpSchedule.Text = "";
                        dtpStartHour.Text = "";
                        dtpEndHour.Text = "";
                        cbStatus.Enabled = true;
                        btnAddEvent.Enabled = true;
                        btnUpdateEvent.Enabled = false;
                        btnDeleteEvent.Enabled = false; 
                        btnViewEvent.Enabled = false;

                    }
                    else
                    {
                        MessageBox.Show("Failed!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    Display_EventList();
                }
            }
        }

        private void btnDeleteEvent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Please select row to from event list!");
            }
            else 
            {
                try
                {
                    connection.Open();
                    command = new MySqlCommand(deleteEventQueryString, connection);
                    command.Parameters.Add("@Id", MySqlDbType.Int32).Value = int.Parse(txtId.Text);

                    int result = command.ExecuteNonQuery();

                    if (result == 1)
                    {
                        MessageBox.Show("Data deleted successfully!");
                        txtEventName.Text = "";
                        txtDescription.Text = "";
                        txtLocation.Text = "";
                        dtpSchedule.Text = "";
                        dtpStartHour.Text = "";
                        dtpEndHour.Text = "";
                        cbStatus.Enabled = true;
                        btnAddEvent.Enabled = true;
                        btnUpdateEvent.Enabled = false;
                        btnDeleteEvent.Enabled = false;
                        btnViewEvent.Enabled = false;

                    }
                    else
                    {
                        MessageBox.Show("Failed!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex);
                }
                finally {
                    command.Dispose();
                    connection.Close();
                    Display_EventList();
                }
            }
        }

        private void btnViewEvent_Click(object sender, EventArgs e)
        {
            btnViewEvent.Enabled = false;
            Attendance attendance = new Attendance(txtId.Text, txtEventName.Text, txtDescription.Text);
            attendance.Show();
        }

        private void dgvEventList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvEventList.SelectedRows.Count > 0)
            {
                selectedRowIndex = e.RowIndex;

                DataGridViewRow selectedDGVRow = dgvEventList.Rows[selectedRowIndex];
                txtId.Text = selectedDGVRow.Cells[0].Value.ToString();
                txtEventName.Text = selectedDGVRow.Cells[1].Value.ToString();
                txtDescription.Text = selectedDGVRow.Cells[2].Value.ToString();
                txtLocation.Text = selectedDGVRow.Cells[3].Value.ToString();
                dtpSchedule.Text = selectedDGVRow.Cells[4].Value.ToString();
                dtpStartHour.Text = selectedDGVRow.Cells[5].Value.ToString();
                dtpEndHour.Text = selectedDGVRow.Cells[6].Value.ToString();
                cbStatus.Text = selectedDGVRow.Cells[7].Value.ToString();
                cbStatus.Enabled = true;
                btnUpdateEvent.Enabled = true;
                btnDeleteEvent.Enabled = true;
                btnAddEvent.Enabled = false;

                Attendance attendance = new Attendance(
                    selectedDGVRow.Cells[0].Value.ToString(),
                    selectedDGVRow.Cells[1].Value.ToString(),
                    selectedDGVRow.Cells[2].Value.ToString()
                    );
                attendance.Show();
             }
        }
    }
}
