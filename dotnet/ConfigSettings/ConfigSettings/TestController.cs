using Microsoft.Extensions.Options;

namespace ConfigSettings
{
    public class TestController
    {
        private readonly IOptionsSnapshot<Config> optionConfig;

        public TestController(IOptionsSnapshot<Config> optionConfig)
        {
            this.optionConfig = optionConfig;
        }

        public void Test()
        {
            Console.WriteLine(this.optionConfig.Value.Age);
            Console.WriteLine("**********************");
            Console.WriteLine(this.optionConfig.Value.Age);
        }
    }
}
