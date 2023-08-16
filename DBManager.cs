using BOL;
using DAL;
using MySql.Data.MySqlClient;


namespace DAL
{
    public class DBmanager
    {
        public static string connString =  @"server=localhost;port=3306;user=root; password=root123;database=dotnetprojs"; 

        public static List<Employee> getAllEmployees(){ 
        
         List<Employee> empList = new List<Employee>();
         
         MySqlConnection conn = new MySqlConnection();

            conn.ConnectionString = connString;

            try { 
                conn.Open(); 

                MySqlCommand cmd = new MySqlCommand(); 

                cmd.Connection = conn;

                string query = "SELECT * FROM employee";

                cmd.CommandText = query;

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    
                   
                    int id = int.Parse(reader["id"].ToString());
                    string firstname = reader["firstname"].ToString();
                    string lastname = reader["lastname"].ToString();
                    Department department = Enum.Parse<Department>(reader["department"].ToString().ToUpper());
                    // DateOnly joiningdate = DateOnly.FromDateTime(DateTime.Parse(reader["joiningdate"].ToString()));  
                    DateOnly joindate =DateOnly.FromDateTime(DateTime.Parse( reader["joindate"].ToString()));  
                    Employee newEmp = new Employee(id,firstname,lastname,department,joindate);
                    empList.Add(newEmp);

                }

            } catch (Exception e){
                Console.WriteLine(e.Message);
            }  finally {
                conn.Close(); 
            }

            return empList;

            }// end of getAllEmployee Method


        public static void insertEmployee(Employee newEmp) {
        
          
            MySqlConnection conn = new MySqlConnection();

            conn.ConnectionString = connString;

            try { 
                            
                conn.Open ();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                string query = "insert into employee  values('" + newEmp.ID + "','" + newEmp.FIRSTNAME + "','"+newEmp.LASTNAME+"','"+newEmp.DEPARTMENT+"','"+newEmp.JOINDATE.ToString("yyyy-MM-dd")+"')";
                cmd.CommandText = query;

                cmd.ExecuteNonQuery();

                 
            }catch (Exception e) { 
            
            Console.WriteLine(e.Message);
            }finally { conn.Close(); }


        }// end of insert employee

        public static void deleteEmployee(int id)
        {

            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = connString;


            try { 
                conn.Open ();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                string query = "delete from employee where id = " + id;
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            
            }catch (Exception e) { 
                Console.WriteLine();
            }finally { conn.Close(); }


        }// end of delete employee



        }
}



public IActionResult Employees()
{
    List<Employee> employees = DBmanager.getAllEmployees();

    ViewBag.Employees = employees;

    return View();
}

public IActionResult Register()
{
    return View();
}

public ActionResult deleteBYId()
{
    return View();
}



[HttpPost]
public ActionResult deleteBYId(int id)
{
    DBmanager.deleteEmployee(id);

    return RedirectToAction("Employees");
}


[HttpPost]
public IActionResult Register(string id, string firstname, string lastname, string department, string joindate)
{
    Employee newEmp = new Employee(int.Parse(id), firstname, lastname, Enum.Parse<Department>(department.ToUpper()), DateOnly.Parse(joindate));

    DBmanager.insertEmployee(newEmp);

    return RedirectToAction("Employees");
}



@using BOL;

@{
    ViewData["Title"] = "Employees";
}

< h1 > Employees List </ h1 >
< style >
    table {
    border - collapse: collapse;
width: 40 %;
border: 1px solid #ccc; /* Add a solid border with a color of your choice */
    }

th, td {
        border: 1px solid #ccc; /* Add a solid border with a color of your choice */
        padding: 8px;
text - align: left;
    }


</ style >
< table >

    < thead style = "border:2;" >
       < tr >
           < th > ID </ th >
           < th > FIRSTNAME </ th >
           < th > LASTNAME </ th >
           < th > DEPARTMENT </ th >
           < th > JOINDATE </ th >
       </ tr >
    </ thead >
    < tbody >
        @{

    @foreach(Employee E in ViewBag.Employees)
            {
               < tr >
               < td > @E.ID </ td >
               < td > @E.FIRSTNAME </ td >
               < td > @E.LASTNAME </ td >
               < td > @E.DEPARTMENT </ td >
               < td > @E.JOINDATE </ td >
               </ tr >
            }
}
    </ tbody >
</ table >


////////////////////////////////////////////////////////////////////////
@using BOL;
@{
    ViewData["Title"] = "Register";
}
< style >
    table {
    border - collapse: collapse;
width: 40 %;
border: 1px solid #ccc; 
    }

th, td {
        border: 1px solid #ccc; 
        padding: 8px;
text - align: left;
    }

</ style >

< div >< h2 > Employee Registration </ h2 ></ div >

< form action = "/Home/Register" method = "post" >
    < table >
        < tr >
            < td > ID : </ td >
            < td >< input type = "text" name = "id" id = "id" required /></ td >
        </ tr >

        < tr >
            < td > FIRSTNAME : </ td >
            < td >< input type = "text" name = "firstname" id = "firstname" required /></ td >
        </ tr >

        < tr >
            < td > LASTNAME : </ td >
            < td >< input type = "text" name = "lastname" id = "lastname" required /></ td >
        </ tr >

        < tr >
            < td > DEPARTMENT : </ td >
            < td >< input type = "text" name = "department" id = "department" required /></ td >
        </ tr >
        < tr >
            < td > JOINDATE : </ td >
            < td >< input type = "date" name = "joindate" id = "joindate" required /></ td >
        </ tr >

        < tr >
            < td ></ td >
            < td >< button type = "submit" id = "btn btn-primary" name = "btn" > Submit </ button ></ td >
        </ tr >
    </ table >
</ form >


/////////////////////////////////////////////////////////////////////////////////////////////

@using BOL;

@{
    ViewData["Title"] = "deleteBYId";
}

< h1 > Delete employee </ h1 >
< style >
    table {
    border - collapse: collapse;
width: 40 %;
border: 1px solid #ccc; 
    }

th, td {
        border: 1px solid #ccc; 
        padding: 8px;
text - align: left;
    }
</ style >
< form action = "/Home/deleteBYId" method = "post" >
    < table >
       < tr >
           < td > Please Enter valid EmpId : </ td >
           < td >< input type = "text" id = "id" name = "id" required /></ td >
       </ tr >
       < tr >
           < td ></ td >
           < td >< button type = "submit" id = "btn" name = "btn" > Delete Employee </ button ></ td >
       </ tr >
    </ table >
</ form >


