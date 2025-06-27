
public class LambdaDemo4
{
    private System.Timers.Timer? _timer;

    // The compiler creates a class for our lambda function.
    private class HiddenClassForLambda
    {
        // The local variable becomes a field of the class.
        public int aVariable;

        // The lambda function becomes a method inside that class.
        public void HiddenMethodForLambda(object? sender, System.Timers.ElapsedEventArgs args)
        {
            Console.WriteLine(aVariable);
        }                                
    }

    public void InitTimer()
    {
        var hiddenObject = new HiddenClassForLambda();

        hiddenObject.aVariable = 5;

        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += hiddenObject.HiddenMethodForLambda;
        _timer.Enabled = true;
    }

}