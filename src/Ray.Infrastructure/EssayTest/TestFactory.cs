using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ray.Infrastructure.EssayTest
{
    public class TestFactory
    {
        /// <summary>
        /// 所有实现ITest接口的类型
        /// </summary>
        private List<Type> _testTypes;

        public TestFactory(Assembly assembly)
        {
            _testTypes = GetTestTypes(assembly);
        }

        /// <summary>
        /// 用于Console程序的循环
        /// </summary>
        public void Run()
        {
            while (true)
            {
                Console.WriteLine($"\r\n请输入测试编号：{this.Selections.AsFormatJsonStr()}");
                string testNum = Console.ReadLine();
                var test = this.Create(testNum);
                test.Run();
            }
        }

        /// <summary>
        /// 创建测试类
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public ITest Create(string num)
        {
            var type = _testTypes.First(x => x.Name.EndsWith(num));
            return (ITest)Activator.CreateInstance(type);
        }

        /// <summary>
        /// 生成测试目录选项
        /// </summary>
        public Dictionary<string, string> Selections => _testTypes
            .ToDictionary(x => x.Name.Substring(x.Name.Length - 2),
                x => x.GetCustomAttribute<DescriptionAttribute>()?.Description ?? x.Name);

        /// <summary>
        /// 获取所有实现ITest接口的类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private List<Type> GetTestTypes(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(x => typeof(ITest).IsAssignableFrom(x))
                .ToList();
        }
    }
}
