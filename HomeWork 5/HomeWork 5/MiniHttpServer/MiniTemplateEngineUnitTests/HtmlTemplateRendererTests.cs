using MiniTemplateEngine.Core;

namespace MiniTemplateEngineUnitTests
{
    [TestClass]
    public sealed class HtmlTemplateRendererTests
    {
        HtmlTemplateRenderer testee = new HtmlTemplateRenderer();
        string? template, expected;

        [TestMethod]
        public void RenderSingleVariable()
        {
            
            template = "<p>Привет, ${Name}</p>";
            var model = new { Name = "Aganiyaz" };
            expected = "<p>Привет, Aganiyaz</p>";
            var result = testee.RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderTwoVariables()
        {
            
            template = "<p>${Name} - ${City}</p>";
            var model = new { Name = "Aganiyaz", City = "Казань" };
            expected = "<p>Aganiyaz - Казань</p>";
            var result = testee.RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderNestedProperty()
        {
            
            template = "<p>Группа: ${Group.Number}</p>";
            var model = new { Group = new { Number = "11-409" } };
            expected = "<p>Группа: 11-409</p>";
            var result = testee.RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderIfTrueSimple()
        {
           
            template = "$if(Name == \"Aganiyaz\")Привет$elseПока$endif";
            var model = new { Name = "Aganiyaz" };
            expected = "Привет";
            var result = testee.RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderIfFalseSimple()
        {
            
            template = "$if(Name == \"Иван\")Да$elseНет$endif";
            var model = new { Name = "Aganiyaz" };
            expected = "Нет";
            var result = testee.RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderIfGreater()
        {
           
            template = "$if(Age > 25)Взрослый$elseМолодой$endif";
            var model = new { Age = 30 };
            expected = "Взрослый";
            var result = testee.RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderIfLess()
        {
            
            template = "$if(Age < 25)Молодой$elseВзрослый$endif";
            var model = new { Age = 30 };
            expected = "Взрослый";
            var result = testee.RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderIfBooleanTrue()
        {
            
            template = "$if(IsAdmin)Да$elseНет$endif";
            var model = new { IsAdmin = true };
            expected = "Да";
            var result = testee.RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderIfBooleanFalse()
        {
            
            template = "$if(IsAdmin)Да$elseНет$endif";
            var model = new { IsAdmin = false };
            expected = "Нет";
            var result = testee .RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderForEachSimple()
        {
            template = "$foreach(var item in Items)<p>${item}</p>$endfor";
            var model = new { Items = new[] { "A", "B", "C" } };
            expected = "<p>A</p><p>B</p><p>C</p>";
            var result = testee.RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderForEachComplex()
        {
            
            template = "$foreach(var task in Tasks)<p>${task.Title} - ${task.Status}</p>$endfor";
            var model = new
            {
                Tasks = new[]
                {
                    new { Title = "Задача1", Status = "Ok" },
                    new { Title = "Задача2", Status = "Fail" }
                }
            };
            expected = "<p>Задача1 - Ok</p><p>Задача2 - Fail</p>";
            var result = testee.RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderNestedIfInForEach()
        {
            template = "$foreach(var task in Tasks)$if(task.Done)Готово$elseВ процессе$endif$endfor";
            var model = new { Tasks = new[] { new { Done = true }, new { Done = false } } };
            expected = "ГотовоВ процессе";
            var result = testee.RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderMultipleVariablesInForEach()
        {
            template = "$foreach(var task in Tasks)<p>${task.Name} - ${task.Status}</p>$endfor";
            var model = new { Tasks = new[] { new { Name = "T1", Status = "Ok" }, new { Name = "T2", Status = "Fail" } } };
            expected = "<p>T1 - Ok</p><p>T2 - Fail</p>";
            var result = testee.RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderIfWithSubProperty()
        {
            template = "$if(Group.Name == \"11-409\")Да$elseНет$endif";
            var model = new { Group = new { Name = "11-409" } };
            expected = "Да";
            var result = testee.RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderEmptyTemplate()
        {
            template = "";
            var model = new { Name = "Aganiyaz" };
            expected = "";
            var result = testee .RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderIfWithMultipleConditions()
        {   template = "$if(Age > 18)Взрослый$else$if(Age > 12)Подросток$elseРебенок$endif$endif";
            var model = new { Age = 15 };
            expected = "Подросток";
            var result = testee.RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderForEachEmpty()
        {
            template = "$foreach(var item in List)<p>${item}</p>$endfor";
            var model = new { List = new string[] { } };
            expected = "";
            var result = testee.RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderVariablesWithSpecialCharacters()
        {
            template = "<p>${Name}@${Domain}</p>";
            var model = new { Name = "Aganiyaz", Domain = "example.ru" };
            string expected = "<p>Aganiyaz@example.ru</p>";
            var result = testee.RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RenderForEachNestedObjects()
        {
            template = "$foreach(var user in Users)<p>${user.Name} - ${user.Group.Name}</p>$endfor";
            var model = new
            {
                Users = new[]
                {
                    new { Name = "Aganiyaz", Group = new { Name = "11-409" } },
                    new { Name = "Boris", Group = new { Name = "11-410" } }
                }
            };
            expected = "<p>Aganiyaz - 11-409</p><p>Boris - 11-410</p>";
            var result = testee.RenderFromString(template, model);
            Assert.AreEqual(expected, result);
        }
    }
}
