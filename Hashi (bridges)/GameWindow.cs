using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Hashi__bridges_
{
    public partial class GameWindow : Form
    {
        private int boardWidth;
        private int boardHeight;
        private BoardGenerator boardGenerator;
        private HashiBoard board;
        private TableLayoutPanel grid; // Keep the grid for layout
        private List<Label> islandLabels = new List<Label>(); // Separate list for island labels

        // Track the first and second selected islands
        private Island firstSelectedIsland;
        private Island secondSelectedIsland;

        private const int DefaultCellSize = 50; // Define a default cell size (50x50 pixels)
        private const int SeparatorThickness = 1; // Define the thickness of the separator between grid cells

        public GameWindow(int height, int width)
        {
            InitializeComponent();

            this.boardHeight = height;
            this.boardWidth = width;

            boardGenerator = new BoardGenerator();
            board = boardGenerator.GenerateSolvableBoard(width, height);

            // Adjust window size based on game dimensions
            AdjustWindowSize();

            CreateGrid();
            CreateIslands();

            // Add reset button
            AddResetButton();
        }

        private void AddResetButton()
        {
            Button resetButton = new Button
            {
                Text = "Reset",
                Size = new Size(80, 30),
                Location = new Point((this.ClientSize.Width - 80) / 2, this.ClientSize.Height - 40) // Center the button below the grid
            };

            // Attach click event handler to reset the game
            resetButton.Click += ResetButton_Click;

            // Add the reset button to the form
            Controls.Add(resetButton);
            resetButton.BringToFront(); // Ensure button stays on top
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                // Check if the current control is a Panel and is not the grid
                if (Controls[i] is Panel panel && panel != grid)
                {
                    Controls.RemoveAt(i);
                    panel.Dispose(); // Dispose the panel after removal
                    i--; // Adjust the index after removing a panel
                }
            }


            foreach (Label islandLabel in islandLabels)
            {
                islandLabel.BackColor = Color.LightBlue;
            }

            for(int i = 0; i < board.Islands.Count; i++)
            {
                
                board.Islands[i].Connections.Clear();
            }

            board.ResetUnavailablePositions();
        }

        private void AdjustWindowSize()
        {
            // Calculate the required client size (inside the form) based on the default cell size and separator thickness
            int gridWidth = boardWidth * DefaultCellSize + (boardWidth - 1) * SeparatorThickness;
            int gridHeight = boardHeight * DefaultCellSize + (boardHeight - 1) * SeparatorThickness;

            int buttonHeight = 40; // Height of the reset button
            int extraSpace = 20; // Extra spacing between grid and button

            // Adjust the form size to fit the grid, button, and account for form borders
            this.ClientSize = new Size(gridWidth, gridHeight + buttonHeight + extraSpace); // Include space for the button below the grid
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Prevent resizing
            this.MaximizeBox = false; // Disable maximize button
        }

        private void CreateGrid()
        {
            // Create a dynamic grid layout for the background grid (no islands in this grid)
            grid = new TableLayoutPanel
            {
                RowCount = boardHeight, // Use the first parameter (height) as the row count
                ColumnCount = boardWidth, // Use the second parameter (width) as the column count
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single, // Add borders for clarity
                Size = new Size(boardWidth * DefaultCellSize + (boardWidth - 1) * SeparatorThickness, boardHeight * DefaultCellSize + (boardHeight - 1) * SeparatorThickness), // Set grid size
                Location = new Point(0, 0) // Start from the top-left
            };

            // Add rows and columns dynamically
            for (int row = 0; row < boardHeight; row++)
            {
                grid.RowStyles.Add(new RowStyle(SizeType.Absolute, DefaultCellSize)); // Fixed row height
            }
            for (int col = 0; col < boardWidth; col++)
            {
                grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, DefaultCellSize)); // Fixed column width
            }

            // Add the grid to the form (background)
            Controls.Add(grid);
            grid.SendToBack(); // Move the grid to the background
        }

        private void CreateIslands()
        {
            // Add islands as labels, positioning them in their respective grid cells
            foreach (var island in board.Islands)
            {
                // Calculate the position for each island within its grid cell
                int islandX = island.X * (DefaultCellSize + SeparatorThickness); // Add separator thickness for X
                int islandY = island.Y * (DefaultCellSize + SeparatorThickness); // Add separator thickness for Y

                // Create a label to represent the island
                Label islandLabel = new Label
                {
                    Text = island.BridgesNeeded.ToString(), // Show the number of bridges needed
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.LightBlue, // Different color for islands
                    BorderStyle = BorderStyle.FixedSingle,
                    Size = new Size(DefaultCellSize, DefaultCellSize), // Set the label size to match the grid cells
                    Location = new Point(islandX, islandY), // Set the island at the exact grid position
                    Tag = island // Store the island data for easy access
                };

                // Make the label a circle by setting its region
                GraphicsPath circlePath = new GraphicsPath();
                circlePath.AddEllipse(0, 0, islandLabel.Width, islandLabel.Height);
                islandLabel.Region = new Region(circlePath);

                // Handle the label's Paint event to draw the text centered in the circle
                islandLabel.Paint += (sender, e) =>
                {
                    Label lbl = sender as Label;
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    // Draw the circular border
                    using (Pen pen = new Pen(Color.Black, 2))
                    {
                        e.Graphics.DrawEllipse(pen, 0, 0, lbl.Width - 1, lbl.Height - 1);
                    }

                    // Draw the text in the center of the circle
                    TextRenderer.DrawText(e.Graphics, lbl.Text, lbl.Font, lbl.ClientRectangle, lbl.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                };


                // Add click event handler for selecting islands
                islandLabel.Click += Island_Click;

                // Add the island label directly to the form (not to the grid)
                Controls.Add(islandLabel);
                islandLabels.Add(islandLabel); // Track all island labels
            }

            // Bring all islands to the front after creating them to ensure they stay on top
            foreach (Label islandLabel in islandLabels)
            {
                islandLabel.BringToFront();
            }
        }

        // Handle the click event for selecting an island
        private void Island_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            Island clickedIsland = clickedLabel?.Tag as Island;

            if (clickedIsland != null)
            {
                if (firstSelectedIsland == null)
                {
                    // Select the first island
                    firstSelectedIsland = clickedIsland;
                    clickedLabel.BackColor = Color.Yellow; // Highlight first selected island
                }
                else if (secondSelectedIsland == null)
                {
                    // Select the second island
                    secondSelectedIsland = clickedIsland;
                    clickedLabel.BackColor = Color.Yellow; // Highlight second selected island

                    // Now try to manage the bridge between the two islands
                    ManageBridgeConnection(firstSelectedIsland, secondSelectedIsland);

                    // Reset the selection
                    ResetSelection();
                }
            }

            if(board.IsSolved())
            {
                MessageBox.Show("Congratulations! You have solved the puzzle!");
            }
        }

        // Manage the bridge connection between two islands
        private void ManageBridgeConnection(Island fromIsland, Island toIsland)
        {
            if (IsValidBridgePath(new Point(fromIsland.X, fromIsland.Y),
                                  new Point(toIsland.X, toIsland.Y)))
            {
                // Check the current number of bridges between the two islands
                int currentBridges = fromIsland.GetBridgeCount(toIsland);

                if (currentBridges == 2)
                {
                    // Two bridges, remove both
                    fromIsland.Disconnect(toIsland); // Remove the connection logically
                    board.RemoveBridgeFromUnavailable(new Point(fromIsland.X, fromIsland.Y),
                                                      new Point(toIsland.X, toIsland.Y)); // Update the unavailable positions
                    RemoveBridgePanel(new Point(fromIsland.X, fromIsland.Y),
                                      new Point(toIsland.X, toIsland.Y)); // Remove the visual representation
                }
                else
                {
                    fromIsland.Connect(toIsland);
                    CreateBridgePanel(new Point(fromIsland.X, fromIsland.Y),
                                      new Point(toIsland.X, toIsland.Y), currentBridges + 1);
                }

                // After managing the connection, update the island colors
                UpdateIslandColor(fromIsland);
                UpdateIslandColor(toIsland);

                //move islands to the front to ensure they stay on top
                foreach (Label islandLabel in islandLabels)
                {
                    islandLabel.BringToFront();
                }

            }
        }

        // Update the island's color based on the number of connected bridges
        private void UpdateIslandColor(Island island)
        {
            // Find the corresponding island label on the form
            Label islandLabel = islandLabels.FirstOrDefault(l => ((Island)l.Tag) == island);

            if (islandLabel != null)
            {
                // Check if the number of bridges matches the required bridgesNeeded
                int connectedBridges = island.Connections.Sum(e => e.BridgeCount);

                if (connectedBridges == island.BridgesNeeded)
                {
                    // Highlight the island in green if the bridge count matches
                    islandLabel.BackColor = Color.Green;
                }
                else if (connectedBridges > island.BridgesNeeded)
                {
                    // Highlight the island in red if the bridge count exceeds the requirement
                    islandLabel.BackColor = Color.Red;
                }
                else if (connectedBridges < island.BridgesNeeded)
                {
                    // Reset the island's color if it doesn't match
                    islandLabel.BackColor = Color.LightBlue;
                }
            }
        }


        // Create a bridge panel between two islands
        private void CreateBridgePanel(Point start, Point end, int bridgeCount)
        {
            board.AddBridgeToUnavailable(start, end); // Update the unavailable positions

            // Get the size of each grid cell from the TableLayoutPanel
            int cellWidth = grid.Width / grid.ColumnCount;
            int cellHeight = grid.Height / grid.RowCount;

            // Calculate the actual pixel positions of the start and end points within the grid
            Point startPosition = new Point(start.X * (cellWidth + SeparatorThickness), start.Y * (cellHeight + SeparatorThickness));
            Point endPosition = new Point(end.X * (cellWidth + SeparatorThickness), end.Y * (cellHeight + SeparatorThickness));

            // Create the consistent tag for the panel using CreateTag
            string tag = CreateTag(start, end);

            // Create a new panel to represent the bridge(s)
            Panel bridgePanel = new Panel
            {
                BackColor = Color.Black, // Bridge color
                Width = (start.X == end.X) ? 5 : Math.Abs(endPosition.X - startPosition.X), // Horizontal or vertical width
                Height = (start.Y == end.Y) ? 5 : Math.Abs(endPosition.Y - startPosition.Y), // Horizontal or vertical height
                Location = new Point(
                    Math.Min(startPosition.X, endPosition.X) + (cellWidth / 2) - SeparatorThickness / 2,
                    Math.Min(startPosition.Y, endPosition.Y) + (cellHeight / 2) - SeparatorThickness / 2
                ), // Adjust the panel's position to center it between the islands, accounting for separators
                Tag = tag // Use CreateTag to create the tag with start and end coordinates
            };

            if (bridgeCount == 2)
            {
                // If there are two bridges, create a parallel bridge
                Panel secondBridgePanel = new Panel
                {
                    BackColor = Color.Black,
                    Width = bridgePanel.Width,
                    Height = bridgePanel.Height,
                    Location = new Point(bridgePanel.Location.X + 10, bridgePanel.Location.Y + 10), // Offset for the second bridge
                    Tag = $"{tag}-2" // Tag the second bridge differently by appending "-2"
                };
                Controls.Add(secondBridgePanel);
                secondBridgePanel.BringToFront();
            }

            // Add the bridge panel to the form
            Controls.Add(bridgePanel);
            bridgePanel.BringToFront(); // Ensure the bridge is drawn on top of the grid
        }


        // Remove the bridge between two islands
        private void RemoveBridgePanel(Point start, Point end)
        {
            // Ensure the tag is consistent regardless of the order of start and end points
            string tagToFind = CreateTag(start, end);
            string tagToFindSecond = $"{tagToFind}-2";

            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i] is Panel panel && (panel.Tag?.ToString() == tagToFind || panel.Tag?.ToString() == tagToFindSecond))
                {
                    Controls.Remove(panel);
                    panel.Dispose(); // Dispose the panel after removal
                    i--; // Adjust the index after removing a panel
                }
            }
        }

        // Helper method to create a consistent tag regardless of order
        private string CreateTag(Point p1, Point p2)
        {
            // Sort points to make the tag consistent, regardless of source/destination order
            if (p1.X < p2.X || (p1.X == p2.X && p1.Y < p2.Y))
            {
                return $"{p1.X},{p1.Y}-{p2.X},{p2.Y}";
            }
            else
            {
                return $"{p2.X},{p2.Y}-{p1.X},{p1.Y}";
            }
        }


        // Reset the selection after a bridge is created
        private void ResetSelection()
        {
            foreach (Label islandLabel in islandLabels)
            {
                // Get the corresponding island object from the label's Tag
                Island island = (Island)islandLabel.Tag;

                // Check the current number of connected bridges for this island
                int connectedBridges = island.Connections.Sum(e => e.BridgeCount);

                // Only reset to blue if the island hasn't met its BridgesNeeded requirement
                if (connectedBridges < island.BridgesNeeded)
                {
                    islandLabel.BackColor = Color.LightBlue; // Reset to blue for unfulfilled islands
                }
            }

            // Clear the selected islands
            firstSelectedIsland = null;
            secondSelectedIsland = null;
        }


        // Check if the path between two points (islands) is straight (horizontal or vertical)
        private bool IsValidBridgePath(Point start, Point end)
        {
            if(start == end)
            {
                return false; // Same island, no bridge needed
            }

            Edge potentialBridge = firstSelectedIsland.Connections.Find(e => e.Destination == secondSelectedIsland);
            if (potentialBridge != null)
            {
                return true;
            }

            if (start.X == end.X)
            {
                int startY = start.Y;
                int endY = end.Y;
                int stepY = Math.Sign(endY - startY);
                for (int i = startY + stepY; i != endY; i += stepY)
                {
                    if (board.UnavailablePositions.Contains((start.X, i)))
                    {
                        return false;
                    }
                }
                return true; // Straight path
            }
            else if (start.Y == end.Y)
            {
                int startX = start.X;
                int endX = end.X;
                int stepX = Math.Sign(endX - startX);
                for (int i = startX + stepX; i != endX; i += stepX)
                {
                    if (board.UnavailablePositions.Contains((i, start.Y)))
                    {
                        return false;
                    }
                }
                return true; // Straight path
            }
            else
            {
                return false; // Not a straight path
            }
        }
    }
}