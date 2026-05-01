using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;

namespace FileManagerP3
{
    public partial class Form1 : Form
    {
        ListBox listBoxFiles;
        RichTextBox textBoxContent;
        Label labelPath;
        FlowLayoutPanel panel;

        private const string FolderPrefix = "[Folder] ";
        private const string SelectFileMsg = "Select a file first.";
        private const string SelectItemMsg = "Select a file or folder first.";
        private const string ConfirmDeleteMsg = "Are you sure you want to delete this?";
        private const string PermissionErrorMsg = "Permission denied.";
        private const string FileNotFoundMsg = "File not found.";
        private const string FileInUseMsg = "Cannot delete. The file or folder is currently in use.";

        string currentPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
    "FileManagerTest"
);
        // Initialize GUI components and layout
        public Form1()
        {
            InitializeComponent();

            labelPath = new Label();
            labelPath.Name = "labelPath";
            labelPath.Text = "Current Path:";
            labelPath.Dock = DockStyle.Top;
            labelPath.Height = 30;
            //this.Controls.Add(labelPath);

            listBoxFiles = new ListBox();
            listBoxFiles.Name = "listBoxFiles";
            listBoxFiles.Width = 250;
            listBoxFiles.Dock = DockStyle.Left;
            //this.Controls.Add(listBoxFiles);

            textBoxContent = new RichTextBox();
            textBoxContent.Name = "textBoxContent";
            textBoxContent.Location = new Point(270, 35);
            textBoxContent.Size = new Size(540, 390);
            textBoxContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxContent.BorderStyle = BorderStyle.FixedSingle;
            textBoxContent.BackColor = Color.White;
            textBoxContent.ForeColor = Color.Black;
            textBoxContent.Font = new Font("Arial", 12);
            textBoxContent.ReadOnly = false;
            textBoxContent.Enabled = true;

            //this.Controls.Add(textBoxContent);

            

            panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Bottom;
            panel.Height = 65;

            string[] btnNames = { "Create", "Read", "Update", "Delete", "Rename", "Copy", "Move" };

            foreach (string name in btnNames)
            {
                Button btn = new Button();
                btn.Text = name;
                btn.Width = 90;

                btn.Click += Button_Click;

                panel.Controls.Add(btn);
            }

            Button backBtn = new Button();
            backBtn.Text = "Back";
            backBtn.Width = 90;

            backBtn.Click += (s, e) =>
            {
                DirectoryInfo parent = Directory.GetParent(currentPath);
                if (parent != null)
                {
                    currentPath = parent.FullName;
                    LoadFiles();
                }
            };

            panel.Controls.Add(backBtn);

            Button folderBtn = new Button();
            folderBtn.Text = "New Folder";
            folderBtn.Width = 100;

            folderBtn.Click += (s, e) =>
            {
                string name = Microsoft.VisualBasic.Interaction.InputBox("Enter folder name:");

                if (string.IsNullOrWhiteSpace(name)) return;

                string folderPath = Path.Combine(currentPath, name); 

                FileOperations.CreateFolder(folderPath);

                LoadFiles();
            };

            panel.Controls.Add(folderBtn);


            this.Controls.Add(labelPath);

            this.Controls.Add(panel);

            
            this.Controls.Add(listBoxFiles);
            this.Controls.Add(textBoxContent);

            listBoxFiles.DoubleClick += ListBoxFiles_DoubleClick;

            listBoxFiles.SelectedIndexChanged += (s, e) =>
            {
                if (listBoxFiles.SelectedItem == null) return;

                string selected = listBoxFiles.SelectedItem.ToString();

                if (!selected.StartsWith(FolderPrefix))
                {
                    string path = Path.Combine(currentPath, selected);

                    if (File.Exists(path))
                    {
                        textBoxContent.Text = File.ReadAllText(path);
                    }
                }
            };


            Directory.CreateDirectory(currentPath);

            LoadFiles();
        }

        

private void ListBoxFiles_DoubleClick(object sender, EventArgs e)
{
    if (listBoxFiles.SelectedItem == null) return;

    string selected = listBoxFiles.SelectedItem.ToString();

    if (selected.StartsWith("[Folder] "))
    {
        string folderName = selected.Replace("[Folder] ", "");
        currentPath = Path.Combine(currentPath, folderName);
        LoadFiles();
    }
}

        // Handles all CRUD operations based on which button is clicked
        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string selected = listBoxFiles.SelectedItem?.ToString();
            string path = "";

            if (selected != null)
            {
                path = Path.Combine(currentPath, selected.Replace("[Folder] ", ""));
            }

            try
            {
                switch (btn.Text)
                {
                    // Create a new file
                    case "Create":
                        string name = Microsoft.VisualBasic.Interaction.InputBox("Enter file name:");

                        if (string.IsNullOrWhiteSpace(name)) return;

                        string newFilePath = Path.Combine(currentPath, name);

                        FileOperations.CreateFile(newFilePath);

                        MessageBox.Show("Created with text at:\n" + newFilePath);

                        LoadFiles();
                        break;

                    // Read and display file contents
                    case "Read":
                        if (string.IsNullOrEmpty(path))
                        {
                            MessageBox.Show(SelectFileMsg);
                            return;
                        }

                        MessageBox.Show("Reading this file:\n" + path);

                        if (!File.Exists(path))
                        {
                            MessageBox.Show("File does not exist.");
                            return;
                        }

                        string content = FileOperations.ReadFile(path);

                        if (string.IsNullOrEmpty(content))
                        {
                            MessageBox.Show("The file is empty.");
                        }

                        textBoxContent.Text = content;

                        
                        FileInfo info = new FileInfo(path);
                        MessageBox.Show($"Size: {info.Length} bytes\nModified: {info.LastWriteTime}");

                        break;

                    // Update file with new content from editor
                    case "Update":
                        if (string.IsNullOrEmpty(path))
                        {
                            MessageBox.Show("Select a file first.");
                            return;
                        }

                        FileOperations.UpdateFile(path, textBoxContent.Text);
                        MessageBox.Show("File updated.");
                        break;

                    // Delete selected file or folder with confirmation
                    case "Delete":
                        {
                            if (string.IsNullOrEmpty(path))
                            {
                                MessageBox.Show(SelectItemMsg);
                                return;
                            }

                            if (MessageBox.Show(ConfirmDeleteMsg, "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                return;
                            }

                            try
                            {
                                string pathToDelete = path;

                                textBoxContent.Clear();
                                listBoxFiles.Items.Clear();

                                if (Directory.Exists(pathToDelete))
                                {
                                    Directory.Delete(pathToDelete, true);
                                }
                                else if (File.Exists(pathToDelete))
                                {
                                    File.Delete(pathToDelete);
                                }

                                MessageBox.Show("Deleted successfully.");
                                LoadFiles();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Delete failed: " + ex.Message);
                                LoadFiles();
                            }

                            break;
                        }

                    // Rename selected file or folder
                    case "Rename":
                        {
                            string newName = Microsoft.VisualBasic.Interaction.InputBox("Enter new file name:");

                            if (string.IsNullOrWhiteSpace(newName)) return;

                            string newPath = Path.Combine(currentPath, newName);

                            FileOperations.RenameItem(path, newPath);

                            MessageBox.Show("File renamed.");
                            LoadFiles();
                            break;
                        }
                    case "Copy":
                        {
                            if (string.IsNullOrEmpty(path))
                            {
                                MessageBox.Show(SelectFileMsg);
                                return;
                            }

                            string folderName = Microsoft.VisualBasic.Interaction.InputBox("Enter destination folder name:");

                            if (string.IsNullOrWhiteSpace(folderName)) return;

                            string destinationFolder = Path.Combine(currentPath, folderName);

                            if (!Directory.Exists(destinationFolder))
                            {
                                MessageBox.Show("Destination folder does not exist.");
                                return;
                            }

                            string newPath = Path.Combine(destinationFolder, Path.GetFileName(path));

                            FileOperations.CopyItem(path, newPath);

                            MessageBox.Show("File copied successfully.");
                            LoadFiles();
                            break;
                        }

                    case "Move":
                        {
                            if (string.IsNullOrEmpty(path))
                            {
                                MessageBox.Show(SelectFileMsg);
                                return;
                            }

                            string folderName = Microsoft.VisualBasic.Interaction.InputBox("Enter destination folder name:");

                            if (string.IsNullOrWhiteSpace(folderName)) return;

                            string destinationFolder = Path.Combine(currentPath, folderName);

                            if (!Directory.Exists(destinationFolder))
                            {
                                MessageBox.Show("Destination folder does not exist.");
                                return;
                            }

                            string newPath = Path.Combine(destinationFolder, Path.GetFileName(path));

                            FileOperations.MoveItem(path, newPath);

                            MessageBox.Show("File moved successfully.");
                            textBoxContent.Clear();
                            LoadFiles();
                            break;
                        }
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(PermissionErrorMsg);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(FileNotFoundMsg);
            }
            catch (IOException)
            {
                MessageBox.Show(FileInUseMsg);
            }
        }

        // Loads all files and folders from the current directory into the list
        private void LoadFiles()
        {
            listBoxFiles.Items.Clear();
            labelPath.Text = "Current Path: " + currentPath;

            string[] files = FileOperations.GetFiles(currentPath);
            string[] folders = FileOperations.GetFolders(currentPath);

            foreach (string folder in folders)
            {
                listBoxFiles.Items.Add("[Folder] " + Path.GetFileName(folder));
            }

            foreach (string file in files)
            {
                listBoxFiles.Items.Add(Path.GetFileName(file));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
