namespace CSharpConcurrency.Chapter3;

public class Chapter3
{
    public static int GetBitmapWidth(string path)
    {
        using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            var fileId = new byte[2];
            var read = file.Read(fileId, 0, 2);          
            
            if (read != 2 || fileId[0] != 'B' || fileId[1] != 'M')      
                throw new Exception("Not a BMP file");                  
        
            file.Seek(18, SeekOrigin.Begin);                  
            
            var widthBuffer = new byte[4];                          
            
            read = file.Read(widthBuffer, 0, 4);        
            
            if(read != 4) 
                throw new Exception("Not a BMP file");    
            
            return BitConverter.ToInt32(widthBuffer, 0);   
        }
    }

    public void GetBitmapWidthAsync(string path, Action<int> setResult, Action<Exception> setException)
    {
        using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            var data = new ClassForGetBitmapWidth();
            data.setResult = setResult;
            data.setException = setException;
            data.file = new FileStream(path, FileMode.Open, FileAccess.Read); 
            try
            {
                data.fileId = new byte[2];                                    
                var read = data.file.ReadAsync(data.fileId, 0, 2).
                ContinueWith(data.GetBitmapWidthStep2);                    
            }
            catch(Exception ex)                                            
            {                                                              
                data.file.Dispose();                                       
                setException(ex);                                          
            }              
        }
    }

    private class ClassForGetBitmapWidth
    {
        public Stream file;
        public byte[] fileId;
        public byte[] widthBuffer;
        public Action<int> setResult;
        public Action<Exception> setException;

        public void GetBitmapWidthStep2(Task<int> task)
        {
            try
            {
                var read = task.Result;
                if (read != 2 || fileId[0] != 'B' || fileId[1] != 'M')
                    throw new Exception("Not a BMP file");

                file.Seek(18, SeekOrigin.Begin);
                widthBuffer = new byte[4];
                file.ReadAsync(widthBuffer, 0, 4).ContinueWith(GetBitmapWidthStep3);
            }
            catch (Exception ex)
            {
                file.Dispose();
                setException(ex);
            }
        }
        
        public void GetBitmapWidthStep3(Task<int> task)
        {
            try
            {
                var read = task.Result;                               
                if(read != 4) throw new Exception("Not a BMP file");  
                file.Dispose();                                       
                var result = BitConverter.ToInt32(widthBuffer, 0);    
                setResult(result);                                    
            }
            catch(Exception ex)                                      
            {                                                        
                file.Dispose();                                      
                setException(ex);                                    
            }                                                        
        }
    }
}