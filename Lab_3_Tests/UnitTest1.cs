using System.Runtime.CompilerServices;
using System.Collections;
using Microsoft.VisualBasic;

namespace Lab_3_Tests
{
    public class UnitTest1
    {
        [Theory]
        [ClassData(typeof(AddingStudentsData))]
        public void AddingStudentsIntoCourseTests(List<Student> students, IProfessor professor)
        {
            bool allAdded = false;

            Departament departament = new Departament("LNU DEKANAT");
            Enrollment enrollment = new Enrollment(departament);
            var course = professor.CreateCourse("First #1", enrollment);

            foreach (var item in students)
            {
                course.AddStudent(item);
            }

            foreach (var item in students)
            {
                if (course.Enrollment.CourseStudents[course.Title].Contains(item))
                    allAdded = true;
                else
                    allAdded = false;
            }

            Assert.True(allAdded);
        }

        [Theory]
        [ClassData(typeof(SameStudentData))]
        public void AddingStudentWithSameId3TimesIntoCourse(Student student, IProfessor professor)
        {
            int expectedCount = 1;

            Departament departament = new Departament("LNU DEKANAT");
            Enrollment enrollment = new Enrollment(departament);
            var course = professor.CreateCourse("First #1", enrollment);

            for (int i = 0; i < 3; i++)
            {
                course.AddStudent(student);
            }

            int actual = course.Enrollment.CourseStudents[course.Title].Count;

            Assert.Equal(actual, expectedCount);
        }

        [Theory]
        [ClassData(typeof(SameStudentData))]
        public void RemovingStudentWithSameId3TimesIntoCourse(Student student, IProfessor professor)
        {
            int expectedCount = 0;

            Departament departament = new Departament("LNU DEKANAT");
            Enrollment enrollment = new Enrollment(departament);
            var course = professor.CreateCourse("First #1", enrollment);

            course.AddStudent(student);
            for (int i = 0; i < 3; i++)
            {
                course.RemoveStudent(student);
            }

            int actual = course.Enrollment.CourseStudents[course.Title].Count;

            Assert.Equal(actual, expectedCount);
        }

        [Theory]
        [ClassData(typeof(RemovingStudentsData))]
        public void RemovingStudentsFromCourse(List<Student> students, IProfessor professor)
        {
            bool allRemoved = false;

            Departament departament = new Departament("LNU DEKANAT");
            Enrollment enrollment = new Enrollment(departament);
            var course = professor.CreateCourse("Hehe", enrollment);

            foreach (var item in students)
            {
                course.AddStudent(item);
            }

            foreach (var item in students)
            {
                course.RemoveStudent(item);
                if (course.Enrollment.CourseStudents[course.Title].Contains(item))
                    allRemoved = false;
                else
                    allRemoved = true;
            }

            Assert.True(allRemoved);
        }

        [Theory]
        [ClassData(typeof(FillingCourseData))]
        public void FillingCourseTests(Group group, IProfessor professor)
        {
            bool allAdded = false;

            Departament departament = new Departament("LNU DEKANAT");
            Enrollment enrollment = new Enrollment(departament);
            var course = professor.CreateCourse("First #1", enrollment);

            professor.FillCourse(group, course);

            foreach (var item in group.Students)
            {
                if (course.Enrollment.CourseStudents[course.Title].Contains(item))
                    allAdded = true;
                else
                    allAdded = false;
            }

            Assert.True(allAdded);
        }

        [Theory]
        [ClassData(typeof(AbstractFactoryData))]
        public void AbstarctFactoryTesting(IProfessor professor)
        {
            Type expected;
            Departament departament = new Departament("123");
            Enrollment enrollment = new Enrollment(departament);

            if (typeof(AlgorithmsProfessor) == professor.GetType())
                expected = typeof(AlgorithmsCourse);
            else if (typeof(ProgrammingProfessor) == professor.GetType())
                expected = typeof(ProgrammingCourse);
            else
                expected = typeof(MathCourse);

            var course = professor.CreateCourse("Amigo", enrollment);
            Type actual = course.GetType();

            Assert.Equal(actual, expected);
        }

        public class AddingStudentsData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                        new List<Student>()
                        {   new Student(1, "Alex"),
                            new Student(2, "Serhiy"),
                            new Student(3, "Alexis"),
                            new Student(4, "Amrica"),
                            new Student(5, "Heeh"),
                        },
                        new MathProfessor(1, "Bengemin", 100),
                };
                yield return new object[]
                {
                        new List<Student>()
                        {   new Student(1, "Alex"),
                            new Student(2, "Serhiy"),
                            new Student(3, "Alexis"),
                            new Student(4, "Amrica"),
                            new Student(5, "Hehe"),
                        },
                        new AlgorithmsProfessor(1, "Bengemin", 100),
                };
                yield return new object[]
                {
                        new List<Student>()
                        {   new Student(1, "Alex"),
                            new Student(2, "Serhiy"),
                            new Student(3, "Alexis"),
                            new Student(4, "Amrica"),
                            new Student(5, "Hehe"),
                        },
                        new ProgrammingProfessor(1, "Bengemin", 100),
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class RemovingStudentsData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                        new List<Student>()
                        {   new Student(1, "Alex"),
                            new Student(2, "Serhiy"),
                            new Student(3, "Alexis"),
                            new Student(4, "Amrica"),
                            new Student(5, "Heeh"),
                        },
                        new MathProfessor(1, "Bengemin", 100),
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class FillingCourseData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Group(1, "Fep-22")
                    {
                        Students = new List<Student>()
                        {   new Student(1, "Alex"),
                            new Student(2, "Serhiy"),
                            new Student(3, "Alexis"),
                            new Student(4, "Amrica"),
                            new Student(5, "Hehe"),
                        }
                    },
                    new MathProfessor(1, "Amigo", 222)
                };
                yield return new object[]
                {
                    new Group(1, "Fep-22")
                    {
                        Students = new List<Student>()
                        {   new Student(1, "Alex"),
                            new Student(2, "Serhiy"),
                            new Student(3, "Alexis"),
                            new Student(4, "Amrica"),
                            new Student(5, "Hehe"),
                        }
                    },
                    new AlgorithmsProfessor(1, "Amigo", 222)
                };
                yield return new object[]
                {
                    new Group(1, "Fep-22")
                    {
                        Students = new List<Student>()
                        {   new Student(1, "Alex"),
                            new Student(2, "Serhiy"),
                            new Student(3, "Alexis"),
                            new Student(4, "Amrica"),
                            new Student(5, "Hehe"),
                        }
                    },
                    new ProgrammingProfessor(1, "Amigo", 222)
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class AbstractFactoryData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new MathProfessor(1, "Amigo", 222)
                };
                yield return new object[]
                {
                    new AlgorithmsProfessor(1, "Amigo", 222)
                };
                yield return new object[]
                {
                    new ProgrammingProfessor(1, "Amigo", 222)
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class SameStudentData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Student(1, "Alex"),
                    new MathProfessor(1, "Amigo", 222)
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}