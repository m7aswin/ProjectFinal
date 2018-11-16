using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Faculty_UploadNotesControl : System.Web.UI.UserControl
{
    public static int x = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        dbconnection db = new dbconnection();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select NoteID from key_gen";
        SqlDataReader dr = db.executeread(cmd);
        dr.Read();
        x = dr.GetInt32(0);
        x++;
        string NID = "NOTE" + x.ToString();
        TextBox1.Text = NID;

        if (!IsPostBack)
        {
            dbconnection db1 = new dbconnection();
            SqlCommand cmd1 = new SqlCommand();
            cmd1.CommandText = "select Course from tblCourse";
            SqlDataReader dr1 = db1.executeread(cmd1);
            while (dr1.Read())
            {
                DropDownList1.Items.Add(dr1.GetString(0).ToString());
            }
        }

        if (!IsPostBack)
        {
            DropDownList2.Items.Add("1");
            DropDownList2.Items.Add("2");
            DropDownList2.Items.Add("3");
            DropDownList2.Items.Add("4");
            DropDownList2.Items.Add("5");
            DropDownList2.Items.Add("6");
            DropDownList2.Items.Add("7");
            DropDownList2.Items.Add("8");
        }

        if (IsPostBack)
        {
            dbconnection db2 = new dbconnection();
            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "select Subject from tblSubject where Course=@c and Semester=@s";
            cmd2.Parameters.AddWithValue("c", DropDownList1.SelectedItem.Text);
            cmd2.Parameters.AddWithValue("s", DropDownList2.SelectedItem.Text);
            SqlDataReader dr2 = db2.executeread(cmd2);
            while (dr2.Read())
            {
                DropDownList3.Items.Add(dr2.GetString(0).ToString());
            }
        }
    }
    protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
    {
        string subject = DropDownList3.SelectedItem.Text;
        dbconnection db3 = new dbconnection();
        SqlCommand cmd3 = new SqlCommand();
        cmd3.CommandText = "select TeacherID from tblSubject where Subject=@id";
        cmd3.Parameters.AddWithValue("@id", subject);
        SqlDataReader dr3 = db3.executeread(cmd3);
        dr3.Read();
        string teach = dr3.GetString(0);
        dbconnection db6 = new dbconnection();
        SqlCommand cmd4 = new SqlCommand();
        cmd4.CommandText = "select Name from tblFaculty where TeacherID=@id";
        cmd4.Parameters.AddWithValue("@id", teach);
        SqlDataReader dr4 = db6.executeread(cmd4);
        dr4.Read();
        Label2.Text = dr4.GetString(0);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        dbconnection db4 = new dbconnection();
        SqlCommand cmd5 = new SqlCommand();

        if (FileUpload1.HasFile)
        {
            string strname = FileUpload1.FileName.ToString();
            FileUpload1.PostedFile.SaveAs(Server.MapPath("~/notes/") + strname);
            cmd5.CommandText = "insert into tblnotes values(@nid,@course,@sem,@sub,@teach,@note,@rem)";
            cmd5.Parameters.AddWithValue("@nid", TextBox1.Text);
            cmd5.Parameters.AddWithValue("@course", DropDownList1.SelectedItem.Text);
            cmd5.Parameters.AddWithValue("@sem", DropDownList2.SelectedItem.Text);
            cmd5.Parameters.AddWithValue("@sub", DropDownList3.SelectedItem.Text);
            cmd5.Parameters.AddWithValue("@teach", Label2.Text);
            cmd5.Parameters.AddWithValue("@note", strname);
            cmd5.Parameters.AddWithValue("@rem", TextBox2.Text);
            db4.execute(cmd5);

            dbconnection db5 = new dbconnection();
            SqlCommand cmd6 = new SqlCommand();
            cmd6.CommandText = "update key_gen set NoteID=@idd";
            cmd6.Parameters.AddWithValue("@idd", x);
            db5.execute(cmd6);
        }
    }
}