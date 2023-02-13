using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace AttendanceAndEventSchedulingSystem
{
    public partial class Attendance : Form
    {
        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=aess_db");
        MySqlCommand command;
        string addStudentString = "INSERT INTO `attendance_list` (`event_id`,`full_name`,`year`,`section`,`course`,`student_status`) " +
            "VALUES (@event_id, @full_name, @year, @section, @course, @student_status)";

        public Attendance(string id = null, string name = null, string description = null)
        {
            InitializeComponent();
            lblAttendId.Text = id;
            lblEventTitle.Text = name;
            lblEventDescription.Text = description;
            label9.Visible = true;  
            label8.Visible = true;
            string retrieveAttendanceDataString = "SELECT id, full_name, year, section, " +
                "course, student_status, created_at FROM attendance_list WHERE event_id = "+ id;

            MySqlDataAdapter adapter;
            DataTable table;
            try
            {
                connection.Open();
                command = new MySqlCommand(retrieveAttendanceDataString, connection);
                adapter = new MySqlDataAdapter(command);
                table = new DataTable();
                adapter.Fill(table);

                dgvAttendView.DataSource = null;
                dgvAttendView.AutoGenerateColumns = false;

                dgvAttendView.ColumnCount = 7;

                dgvAttendView.Columns[0].Name = "id";
                dgvAttendView.Columns[0].HeaderText = "Id";
                dgvAttendView.Columns[0].DataPropertyName = "id";
                dgvAttendView.Columns[0].Width = 20;

                dgvAttendView.Columns[1].Name = "full_name";
                dgvAttendView.Columns[1].HeaderText = "Full Name";
                dgvAttendView.Columns[1].DataPropertyName = "full_name";
                dgvAttendView.Columns[1].Width = 150;

                dgvAttendView.Columns[2].Name = "year";
                dgvAttendView.Columns[2].HeaderText = "Year";
                dgvAttendView.Columns[2].DataPropertyName = "year";
                dgvAttendView.Columns[2].Width = 50;

                dgvAttendView.Columns[3].Name = "section";
                dgvAttendView.Columns[3].HeaderText = "Section";
                dgvAttendView.Columns[3].DataPropertyName = "section";
                dgvAttendView.Columns[3].Width = 50;

                dgvAttendView.Columns[4].Name = "course";
                dgvAttendView.Columns[4].HeaderText = "Course";
                dgvAttendView.Columns[4].DataPropertyName = "course";
                dgvAttendView.Columns[4].Width = 80;

                dgvAttendView.Columns[5].Name = "student_status";
                dgvAttendView.Columns[5].HeaderText = "Status";
                dgvAttendView.Columns[5].DataPropertyName = "student_status";
                dgvAttendView.Columns[5].Width = 80;

                dgvAttendView.Columns[6].Name = "created_at";
                dgvAttendView.Columns[6].HeaderText = "Created Date";
                dgvAttendView.Columns[6].DataPropertyName = "created_at";
                dgvAttendView.Columns[6].Width = 110;

                dgvAttendView.DataSource = table;
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
        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAttendName.Text) || string.IsNullOrEmpty(cbAttendYear.Text) || string.IsNullOrEmpty(cbAttendSection.Text) || string.IsNullOrEmpty(cbAttendCourse.Text) || string.IsNullOrEmpty(cbAttendStatus.Text))
            {
                if (string.IsNullOrEmpty(txtAttendName.Text))
                {
                    MessageBox.Show("Full Name is required!");
                }
                else if (string.IsNullOrEmpty(cbAttendYear.Text))
                {
                    MessageBox.Show("Year is required!");
                }
                else if (string.IsNullOrEmpty(cbAttendSection.Text))
                {
                    MessageBox.Show("Section is required!");
                }
                else if (string.IsNullOrEmpty(cbAttendCourse.Text))
                {
                    MessageBox.Show("Course is required!");
                }
                else if (string.IsNullOrEmpty(cbAttendStatus.Text))
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
                    command = new MySqlCommand(addStudentString, connection);
                    command.Parameters.Add("@event_id", MySqlDbType.VarChar).Value = lblAttendId.Text;
                    command.Parameters.Add("@full_name", MySqlDbType.VarChar).Value = txtAttendName.Text;
                    command.Parameters.Add("@year", MySqlDbType.VarChar).Value = cbAttendYear.Text;
                    command.Parameters.Add("@section", MySqlDbType.VarChar).Value = cbAttendSection.Text;
                    command.Parameters.Add("@course", MySqlDbType.VarChar).Value = cbAttendCourse.Text;
                    command.Parameters.Add("@student_status", MySqlDbType.VarChar).Value = cbAttendStatus.Text;

                    int result = command.ExecuteNonQuery();

                    if (result == 1)
                    {
                        MessageBox.Show("Data added successfully!");
                        txtAttendName.Text = "";
                        cbAttendYear.Text = "";
                        cbAttendSection.Text = "";
                        cbAttendCourse.Text = "";
                        cbAttendStatus.Text = "";
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
                    this.Hide();
                    Attendance attendance = new Attendance(
                        lblAttendId.Text,
                        lblEventTitle.Text,
                        lblEventDescription.Text
                        );
                    attendance.Show();
                }
            }
        }
    }
}
