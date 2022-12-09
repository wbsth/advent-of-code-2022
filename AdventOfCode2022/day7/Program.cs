string filePath = @"./input.txt";

if (!File.Exists(filePath))
{
    Console.WriteLine("no file found");
    return;
}

var lines = File.ReadAllLines(filePath);

MyDirectory? currentDirectory = null;
MyDirectory? mainDirectory = null;

foreach (var line in lines)
{
    // recognize command
    var txtSplitted = line.Split(" ");

    if ((txtSplitted)[0] == "$")
    {
        if (txtSplitted[1] == "cd")
        {
            if (currentDirectory == null)
            {
                currentDirectory = new MyDirectory(txtSplitted[2], null);
                mainDirectory = currentDirectory;
                continue;
            }
            else if (txtSplitted[2] == "..")
            {
                // go back
                currentDirectory = currentDirectory.ParentDirectory;
            }
            else
            {
                // go to child
                currentDirectory = currentDirectory.Directories.First(x => x.Name == txtSplitted[2]);
            }
        }
        else if(txtSplitted[1] == "ls")
        {
            // do nothing??
        }
    }
    else
    {
        currentDirectory.addElement(line); 
    }
}

var allDirectories = listAllDirectoriesRecursively(mainDirectory);

var smallDirectories = allDirectories.Where(x => x.Size <= 100000);
var smallDirectoriesSumSize = smallDirectories.Select(x => x.Size).Sum();

Console.WriteLine($"Sum of size of small dirs: {smallDirectoriesSumSize}");

var totalSize = mainDirectory.Size;
var freeSpaceAvailable = 70000000 - totalSize;
var extraFreeSpaceNeeded = 30000000 - freeSpaceAvailable;
var dirToDelete = allDirectories.Where(x => x.Size > extraFreeSpaceNeeded).OrderBy(x=>x.Size).First();

Console.WriteLine($"Size of dir to delete: {dirToDelete.Size}");

static List<MyDirectory> listAllDirectoriesRecursively(MyDirectory myDirectory)
{
    var directoriesList = new List<MyDirectory>();
    directoriesList.Add(myDirectory);
    
    foreach (var directory in myDirectory.Directories)
    {
        directoriesList.AddRange(listAllDirectoriesRecursively(directory));
    }

    return directoriesList;
}

public class MyDirectory:Child
{
    public MyDirectory(string name, MyDirectory parent)
    {
        Name = name;
        ParentDirectory = parent;
    }
    public List<MyFile> Files { get; set; }= new List<MyFile>();
    public List<MyDirectory> Directories { get; set; }= new List<MyDirectory>();

    public int Size
    {
        get
        {
            return Files.Select(x => x.Size).Sum()
                + Directories.Select(x=>x.Size).Sum();
        }
    }

    public void addElement(string txt)
    {
        var txtSplitted = txt.Split(" ");
        if (txtSplitted[0] == "dir")
        {
            Directories.Add(new MyDirectory(txtSplitted[1], this));
        }
        else
        {
            var fileSize = int.Parse(txtSplitted[0]);
            var fileName = txtSplitted[1];
            Files.Add(new MyFile(fileName, fileSize, this));
        }
    }
}

public class MyFile:Child
{
    public MyFile(string name, int size, MyDirectory parent)
    {
        Size = size;
        Name = name;
        ParentDirectory = parent;
    }
    public int Size { get; set; }
}

public class Child
{
    public string Name { get; set; } = "";
    public MyDirectory? ParentDirectory { get; set; }
}
