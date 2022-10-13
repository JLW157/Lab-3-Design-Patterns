using Lab_2_v2.Extensions;

MathProfessor mathProfessor = new MathProfessor(1, "Slaex", 100);
ProgrammingProfessor progProfessor = new ProgrammingProfessor(2, "Bjerne", 250);
AlgorithmsProfessor algorithmsProfessor = new AlgorithmsProfessor(3, "Andriy", 200);

Student student = new Student(1, "Sasha");
Student student2 = new Student(2, "Amigo");

Group group = new Group(1, "FEP-22");
group.Students.Add(student);
group.Students.Add(student2);

Departament departament = new Departament("Lnu dekanat");
Enrollment enrollment = new Enrollment(departament);

ICourse mathCourse = mathProfessor.CreateCourse("Vishka", enrollment);
ICourse progCourse = progProfessor.CreateCourse("Software Engineering", enrollment);
ICourse algoCourse = algorithmsProfessor.CreateCourse("Algorithms", enrollment);

progProfessor.FillCourse(group, progCourse);
mathProfessor.FillCourse(group, mathCourse);

progCourse.RemoveStudent(student);
progCourse.RemoveStudent(student);

mathCourse.AddStudent(student);
mathCourse.AddStudent(student); //Already added

progCourse.AddStudent(student);
progCourse.AddStudent(student); //Already added

algoCourse.AddStudent(student);
algoCourse.AddStudent(student2);
algoCourse.AddStudent(student); //Already added

enrollment.ShowInfo();
departament.ShowInfo();

public interface ICourse
{
    public string Title { get; set; }

    public void AddStudent(Student student);
    public void RemoveStudent(Student student);
    public Enrollment Enrollment { get; set; }
}

public interface IStaff
{
    public PersonalInfo PersonalInfo { get; set; }
    public abstract bool AskForLeave(Departament departament, Request request);
    public abstract bool SendRequest(Departament departament, Request request);
}

public class PersonalInfo
{
    public PersonalInfo() { }

    public PersonalInfo(int id, string Name, float salary = 0)
    {
        this.Id = id;
        this.Name = Name;
        this.Salary = salary;
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public float Salary { get; set; }
}


public class MathCourse : ICourse
{
    public MathCourse(string title, Enrollment enrollment)
    {
        this.Title = title;
        this.Enrollment = enrollment;
    }
    public string Title { get; set; } = string.Empty;
    public Enrollment Enrollment { get; set; }

    public void AddStudent(Student student)
    {
        this.Enrollment.Enroll(student, this);
    }

    public void RemoveStudent(Student student)
    {
        this.Enrollment.Unenroll(student, this);
    }
}

public class ProgrammingCourse : ICourse
{
    public ProgrammingCourse(string title, Enrollment enrollment)
    {
        this.Title = title;
        this.Enrollment = enrollment;
    }
    public string Title { get; set; } = string.Empty;
    public Enrollment Enrollment { get; set; }

    public void AddStudent(Student student)
    {
        this.Enrollment.Enroll(student, this);
    }

    public void RemoveStudent(Student student)
    {
        this.Enrollment.Unenroll(student, this);
    }
}

public class AlgorithmsCourse : ICourse
{
    public AlgorithmsCourse(string title, Enrollment enrollment)
    {
        this.Title = title;
        this.Enrollment = enrollment;
    }
    public string Title { get; set; } = string.Empty;
    public Enrollment Enrollment { get; set; }

    public void AddStudent(Student student)
    {
        this.Enrollment.Enroll(student, this);
    }

    public void RemoveStudent(Student student)
    {
        this.Enrollment.Unenroll(student, this);
    }
}

public class Enrollment
{
    public Enrollment(Departament departament)
    {
        this.Departament = departament;
    }

    public Departament Departament { get; set; }

    private List<ICourse> Courses { get; set; } = new List<ICourse>();
    private List<Student> Students { get; set; } = new List<Student>();
    private List<IProfessor> Professors { get; set; } = new List<IProfessor>();

    public Dictionary<string, List<Student>> CourseStudents { get; set; } = new Dictionary<string, List<Student>>();
    public Dictionary<int, List<ICourse>> StudentsCourse { get; set; } = new Dictionary<int, List<ICourse>>();

    public Dictionary<string, IProfessor> CourseAndProfessor { get; set; } = new Dictionary<string, IProfessor>();
    public Dictionary<int, List<ICourse>> ProfessorAndCourses { get; set; } = new Dictionary<int, List<ICourse>>();
    
    public Dictionary<string, List<Group>> CourseAndGroups { get; set; } = new Dictionary<string, List<Group>>();
    
    public void Enroll(Student student, ICourse course)
    {
        if (this.CourseStudents.AddCustom(course.Title, student) && this.StudentsCourse.AddCustom(student.PersonalInfo.Id, course))
        {
            Console.WriteLine($"Student {student.PersonalInfo.Name} successfully enroled on course - {course.Title}");
            if (this.Students.Contains(student) == false)
                this.Students.Add(student);

            if (this.Courses.Contains(course) == false)
                this.Courses.Add(course);
        }
        else
            Console.WriteLine($"Student has already enroled on this course");

        if (this.Departament.Students.Contains(student) == false)
            this.Departament.Students.Add(student);

        if (this.Departament.CoursesByTitle.Contains(course.Title) == false)
            this.Departament.CoursesByTitle.Add(course.Title);
    }

    public void Unenroll(Student student, ICourse course)
    {
        if (this.CourseStudents.RemoveCustom(course.Title, student) && this.StudentsCourse.RemoveCustom(student.PersonalInfo.Id, course))
            Console.WriteLine($"Student {student.PersonalInfo.Name} successfully unenrolled");
        else
            Console.WriteLine($"Student or course not found :(");
    }

    public void ShowInfo()
    {
        Console.WriteLine("Courses and his students");
        foreach (var item in this.CourseStudents.Keys)
        {
            Console.WriteLine($"\tCourse title {item}");
            Console.WriteLine($"\tProfessor of course: Name - " +
                $"{this.CourseAndProfessor[item].PersonalInfo.Name} " +
                $"| Salary - {this.CourseAndProfessor[item].PersonalInfo.Salary}");


            Console.WriteLine("\t\tStudents of this course:");
            foreach (var student in this.CourseStudents[item])
            {
                Console.WriteLine($"\t\t\tId: {student.PersonalInfo.Id}, Name: {student.PersonalInfo.Name}");
            }
        }
    }
}

public class Student : IStaff
{
    public Student(int id, string name)
    {
        this.PersonalInfo.Id = id;
        this.PersonalInfo.Name = name;
    }

    public PersonalInfo PersonalInfo { get; set; } = new PersonalInfo();

    public bool AskForLeave(Departament departament, Request request)
    {
        if (departament.Requests.ContainsKey(request.Id) == false)
        {
            departament.Requests.Add(request.Id, request);
            return departament.ProceedRequest(request.Id);
        }

        return departament.ProceedRequest(request.Id);
    }

    public bool SendRequest(Departament departament, Request request)
    {
        if (departament.Requests.ContainsKey(request.Id) == false)
        {
            departament.Requests.Add(request.Id, request);
            return departament.ProceedRequest(request.Id);
        }

        return departament.ProceedRequest(request.Id);
    }
}

public interface IProfessor : IStaff
{
    public bool FillCourse(Group group, ICourse course);
    public ICourse CreateCourse(string Title, Enrollment enrollment);
}

public class MathProfessor : IProfessor
{
    public MathProfessor(int id, string name, float salary)
    {
        this.PersonalInfo.Id = id;
        this.PersonalInfo.Name = name;
        this.PersonalInfo.Salary = salary;
    }

    public PersonalInfo PersonalInfo { get; set; } = new PersonalInfo();

    public ICourse CreateCourse(string title, Enrollment enrollment)
    {
        var course = new MathCourse(title, enrollment);

        course.Enrollment.ProfessorAndCourses.AddCustom(this.PersonalInfo.Id, course);
        course.Enrollment.CourseAndProfessor.AddCustom(course.Title, this);

        if (course.Enrollment.Departament.Professors.Contains(this) == false)
            course.Enrollment.Departament.Professors.Add(this);
        
        return course;
    }

    public bool FillCourse(Group group, ICourse mathCourse1)
    {
        foreach (var item in group.Students)
        {
            mathCourse1.AddStudent(item);
        }
        return true;
    }

    public bool AskForLeave(Departament departament, Request request)
    {
        if (departament.Requests.ContainsKey(request.Id) == false)
        {
            departament.Requests.Add(request.Id, request);
            return departament.ProceedRequest(request.Id);
        }

        return departament.ProceedRequest(request.Id);
    }

    public bool SendRequest(Departament departament, Request request)
    {
        if (departament.Requests.ContainsKey(request.Id) == false)
        {
            departament.Requests.Add(request.Id, request);
            return departament.ProceedRequest(request.Id);
        }

        return departament.ProceedRequest(request.Id);
    }
}

public class ProgrammingProfessor : IProfessor
{
    public ProgrammingProfessor(int id, string name, float salary)
    {
        this.PersonalInfo.Id = id;
        this.PersonalInfo.Name = name;
        this.PersonalInfo.Salary = salary;
    }

    public PersonalInfo PersonalInfo { get; set; } = new PersonalInfo();

    public ICourse CreateCourse(string title, Enrollment enrollment)
    {
        var course = new ProgrammingCourse(title, enrollment);

        course.Enrollment.ProfessorAndCourses.AddCustom(this.PersonalInfo.Id, course);
        course.Enrollment.CourseAndProfessor.AddCustom(course.Title, this);

        if (course.Enrollment.Departament.Professors.Contains(this) == false)
            course.Enrollment.Departament.Professors.Add(this);

        return course;
    }

    public bool FillCourse(Group group, ICourse progCourse)
    {
        foreach (var item in group.Students)
        {
            progCourse.AddStudent(item);
        }
        return true;
    }

    public bool AskForLeave(Departament departament, Request request)
    {
        if (departament.Requests.ContainsKey(request.Id) == false)
        {
            departament.Requests.Add(request.Id, request);
            return departament.ProceedRequest(request.Id);
        }

        return departament.ProceedRequest(request.Id);
    }

    public bool SendRequest(Departament departament, Request request)
    {
        if (departament.Requests.ContainsKey(request.Id) == false)
        {
            departament.Requests.Add(request.Id, request);
            return departament.ProceedRequest(request.Id);
        }

        return departament.ProceedRequest(request.Id);
    }
}

public class AlgorithmsProfessor : IProfessor
{
    public AlgorithmsProfessor(int id, string name, float salary)
    {
        this.PersonalInfo.Id = id;
        this.PersonalInfo.Name = name;
        this.PersonalInfo.Salary = salary;
    }

    public PersonalInfo PersonalInfo { get; set; } = new PersonalInfo();

    public ICourse CreateCourse(string title, Enrollment enrollment)
    {
        var course = new AlgorithmsCourse(title, enrollment);

        course.Enrollment.ProfessorAndCourses.AddCustom(this.PersonalInfo.Id, course);
        course.Enrollment.CourseAndProfessor.AddCustom(course.Title, this);

        if (course.Enrollment.Departament.Professors.Contains(this) == false)
            course.Enrollment.Departament.Professors.Add(this);

        return course;
    }

    public bool FillCourse(Group group, ICourse algoCourse)
    {
        foreach (var item in group.Students)
        {
            algoCourse.AddStudent(item);
        }
        return true;
    }

    public bool AskForLeave(Departament departament, Request request)
    {
        if (departament.Requests.ContainsKey(request.Id) == false)
        {
            departament.Requests.Add(request.Id, request);
            return departament.ProceedRequest(request.Id);
        }

        return departament.ProceedRequest(request.Id);
    }

    public bool SendRequest(Departament departament, Request request)
    {
        if (departament.Requests.ContainsKey(request.Id) == false)
        {
            departament.Requests.Add(request.Id, request);
            return departament.ProceedRequest(request.Id);
        }

        return departament.ProceedRequest(request.Id);
    }
}

public class Group
{
    public Group(int id, string title)
    {
        Id = id;
        Title = title;
    }

    public int Id { get; set; }
    public int DepartamentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<Student> Students { get; set; } = new List<Student>();
}

public class Request
{
    public Request(int id, string message)
    {
        this.Id = id;
        this.Message = message;
    }
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsDone { get; set; } = false;
}

public class Departament
{
    public Departament(string title)
    {
        this.Title = title;
    }

    public string Title { get; set; } = string.Empty;
    public List<Student> Students { get; set; } = new List<Student>();
    public List<IProfessor> Professors { get; set; } = new List<IProfessor>();
    public List<string> CoursesByTitle { get; set; } = new List<string>();
    public Dictionary<int, Request> Requests { get; set; } = new Dictionary<int, Request>();

    public bool ProceedRequests()
    {
        var newList = this.Requests.Where(x => x.Value.IsDone == false);
        if (newList != null)
        {
            foreach (var item in newList.ToList())
                this.Requests[item.Key].IsDone = true;

            return true;
        }

        return false;
    }

    public bool ProceedRequest(int requestId)
    {
        if (this.Requests.ContainsKey(requestId))
        {
            int number = new Random().Next(1, 3);
            switch (number)
            {
                case 1:
                    this.Requests[requestId].IsDone = true;
                    break;
                case 2:
                    this.Requests[requestId].IsDone = false;
                    break;
            }

            return this.Requests[requestId].IsDone;
        }

        return false;
    }

    public void ShowInfo()
    {
        Console.WriteLine($"Departament {this.Title} info");
        Console.WriteLine("Students: ");
        foreach (var item in this.Students)
        {
            Console.WriteLine($"\t{item.PersonalInfo.Name}");
        }
        Console.WriteLine();
        Console.WriteLine("Professors: ");
        foreach (var item in this.Professors)
        {
            Console.WriteLine($"\t{item.PersonalInfo.Name}");
        }
        Console.WriteLine();
        Console.WriteLine("Courses: ");
        foreach (var item in this.CoursesByTitle)
        {
            Console.WriteLine($"\t{item}");
        }

        Console.WriteLine("Requests: ");
        foreach (var item in this.Requests)
        {
            Console.WriteLine($"\t{item.Key} - Message: {item.Value.Message}, Is done: {item.Value.IsDone}");
        }
    }
}