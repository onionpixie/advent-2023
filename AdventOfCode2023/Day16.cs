namespace AdventOfCode
{
    public class Day16 : IDay
    {
        private string[] Lines { get; set; }

        private const char EmptySpace = '.';

        private const char RightMirror = '/';

        private const char LeftMirror = '\\';

        private const char VerticalSplitter = '|';

        private const char HorizontalSplitter = '-';

        private const char LightUp = '^';

        private const char LightDown = 'V';

        private const char LightLeft = '<';

        private const char LightRight = '>';

        public Day16() {
            Lines = File.ReadAllLines("./Day16.txt");
        }

        public string SolveA ()
        {
            var rows = Lines.Length;
            var columns = Lines.First().Length;

            var lightGrid = new char[rows, columns];
            var mirrorGrid = new char[rows, columns];
            for (int i = 0; i < Lines.Length; i++)
            {
                var chars = Lines[i].ToCharArray();
                for (int j = 0; j < chars.Length; j++)
                {
                    mirrorGrid[i, j] = chars[j];
                    lightGrid[i, j] = EmptySpace;
                }
            }

            var lightPaths = new List<char[,]>();
            var firstLightPath = lightGrid.Clone() as char[,];
            var initalDirection = LightRight;
            switch (mirrorGrid[0,0]) {
                case EmptySpace:
                case HorizontalSplitter:
                break;
                case LeftMirror:
                case VerticalSplitter:
                    initalDirection = LightDown;
                break;
                case RightMirror:
                    initalDirection = LightUp;
                break;
            }
            firstLightPath[0, 0] = initalDirection;
            lightPaths.Add(firstLightPath);
            var startPoint = new StartPoint { Row = 0, Column = 0, LightDirection = initalDirection };
            var startPoints = new List<StartPoint>() { startPoint };

            var newStartPoints = WorkOutLightPath(rows, columns, mirrorGrid, firstLightPath, startPoint);
            CheckNewStartPointsAndResolve(rows, columns, lightGrid, mirrorGrid, startPoints, newStartPoints, lightPaths);

            var count = 0;
            for (int i = 0; i < Lines.Length; i++)
            {
                var chars = Lines[i].ToCharArray();
                for (int j = 0; j < chars.Length; j++)
                {
                    if (lightPaths.Any(x => x[i,j] != '.')){
                        count += 1;
                    }
                }
            }

            return count.ToString();
        }

        private static void CheckNewStartPointsAndResolve(int rows, int columns, char[,] lightGrid, char[,] mirrorGrid, List<StartPoint> startPoints, List<StartPoint> newStartPoints, List<char[,]> lightPaths)
        {
            foreach (var newStartPoint in newStartPoints)
            {
                var existing = startPoints.SingleOrDefault(x => x.Row == newStartPoint.Row && x.Column == newStartPoint.Column && x.LightDirection == newStartPoint.LightDirection);
                if (existing == null) {
                    startPoints.Add(newStartPoint);
                    var lightPath = lightGrid.Clone() as char[,];
                    lightPath[newStartPoint.Row, newStartPoint.Column] = newStartPoint.LightDirection;
                    var nextStartPoints = WorkOutLightPath(rows, columns, mirrorGrid, lightPath, newStartPoint);
                    lightPaths.Add(lightPath);
                    CheckNewStartPointsAndResolve(rows, columns, lightGrid, mirrorGrid, startPoints, nextStartPoints, lightPaths);
                }
            }
        }

        private static List<StartPoint> WorkOutLightPath(int rows, int columns, char[,] mirrorGrid, char[,] currentLightPath, StartPoint startPoint)
        {
            var lightNotExitedOrLooped = true;
            var currentCol = startPoint.Column;
            var currentRow = startPoint.Row;
            var startPoints = new List<StartPoint>();
            while (lightNotExitedOrLooped)
            {
                var currentCell = currentLightPath[currentRow, currentCol];
                switch (currentCell)
                {
                    case LightUp:
                        if (currentRow == 0)
                        {
                            lightNotExitedOrLooped = false;
                            continue;
                        }
                        else
                        {
                            currentRow -= 1;
                            var nextCell = mirrorGrid[currentRow, currentCol];
                            var haveWeBeenHereBefore = currentLightPath[currentRow, currentCol] != EmptySpace;
                            switch (nextCell)
                            {
                                case EmptySpace:
                                case VerticalSplitter:
                                    if (haveWeBeenHereBefore)
                                    {
                                        if (HaveWeLooped(currentCol, currentRow, currentLightPath, LightUp))
                                        {
                                            lightNotExitedOrLooped = false;
                                            continue;
                                        }
                                    }
                                    currentLightPath[currentRow, currentCol] = LightUp;
                                    break;
                                case LeftMirror:
                                    if (haveWeBeenHereBefore)
                                    {
                                        if (HaveWeLooped(currentCol, currentRow, currentLightPath, LightLeft))
                                        {
                                            lightNotExitedOrLooped = false;
                                            continue;
                                        }
                                    }
                                    currentLightPath[currentRow, currentCol] = LightLeft;
                                    break;
                                case RightMirror:
                                    if (haveWeBeenHereBefore)
                                    {
                                        if (HaveWeLooped(currentCol, currentRow, currentLightPath, LightRight))
                                        {
                                            lightNotExitedOrLooped = false;
                                            continue;
                                        }
                                    }
                                    currentLightPath[currentRow, currentCol] = LightRight;
                                    break;
                                case HorizontalSplitter:
                                    if (haveWeBeenHereBefore)
                                    {
                                        if (HaveWeLooped(currentCol, currentRow, currentLightPath, LightLeft))
                                        {
                                            lightNotExitedOrLooped = false;
                                            continue;
                                        }
                                    }
                                    currentLightPath[currentRow, currentCol] = LightLeft;
                                    startPoints.Add(new StartPoint {
                                        Row = currentRow,
                                        Column = currentCol,
                                        LightDirection = LightRight
                                    });
                                    break;
                            }
                        }
                        break;
                    case LightDown:
                        if (currentRow == rows - 1)
                        {
                            lightNotExitedOrLooped = false;
                            continue;
                        }
                        else
                        {
                            currentRow += 1;
                            var nextCell = mirrorGrid[currentRow, currentCol];
                            var haveWeBeenHereBefore = currentLightPath[currentRow, currentCol] != EmptySpace;
                            switch (nextCell)
                            {
                                case EmptySpace:
                                case VerticalSplitter:
                                    if (haveWeBeenHereBefore)
                                    {
                                        if (HaveWeLooped(currentCol, currentRow, currentLightPath, LightDown))
                                        {
                                            lightNotExitedOrLooped = false;
                                            continue;
                                        }
                                    }
                                    currentLightPath[currentRow, currentCol] = LightDown;
                                    break;
                                case LeftMirror:
                                    if (haveWeBeenHereBefore)
                                    {
                                        if (HaveWeLooped(currentCol, currentRow, currentLightPath, LightRight))
                                        {
                                            lightNotExitedOrLooped = false;
                                            continue;
                                        }
                                    }
                                    currentLightPath[currentRow, currentCol] = LightRight;
                                    break;
                                case RightMirror:
                                    if (haveWeBeenHereBefore)
                                    {
                                        if (HaveWeLooped(currentCol, currentRow, currentLightPath, LightLeft))
                                        {
                                            lightNotExitedOrLooped = false;
                                            continue;
                                        }
                                    }
                                    currentLightPath[currentRow, currentCol] = LightLeft;
                                    break;
                                case HorizontalSplitter:
                                    if (haveWeBeenHereBefore)
                                    {
                                        if (HaveWeLooped(currentCol, currentRow, currentLightPath, LightLeft))
                                        {
                                            lightNotExitedOrLooped = false;
                                            continue;
                                        }
                                    }
                                    currentLightPath[currentRow, currentCol] = LightLeft;
                                    startPoints.Add(new StartPoint {
                                        Row = currentRow,
                                        Column = currentCol,
                                        LightDirection = LightRight
                                    });
                                    break;
                            }
                        }
                        break;
                    case LightLeft:
                        if (currentCol == 0)
                        {
                            lightNotExitedOrLooped = false;
                            continue;
                        }
                        else
                        {
                            currentCol -= 1;
                            var nextCell = mirrorGrid[currentRow, currentCol];
                            var haveWeBeenHereBefore = currentLightPath[currentRow, currentCol] != EmptySpace;
                            switch (nextCell)
                            {
                                case EmptySpace:
                                case HorizontalSplitter:
                                    if (haveWeBeenHereBefore)
                                    {
                                        if (HaveWeLooped(currentCol, currentRow, currentLightPath, LightLeft))
                                        {
                                            lightNotExitedOrLooped = false;
                                            continue;
                                        }
                                    }
                                    currentLightPath[currentRow, currentCol] = LightLeft;
                                    break;
                                case LeftMirror:
                                    if (haveWeBeenHereBefore)
                                    {
                                        if (HaveWeLooped(currentCol, currentRow, currentLightPath, LightUp))
                                        {
                                            lightNotExitedOrLooped = false;
                                            continue;
                                        }
                                    }
                                    currentLightPath[currentRow, currentCol] = LightUp;
                                    break;
                                case RightMirror:
                                    if (haveWeBeenHereBefore)
                                    {
                                        if (HaveWeLooped(currentCol, currentRow, currentLightPath, LightDown))
                                        {
                                            lightNotExitedOrLooped = false;
                                            continue;
                                        }
                                    }
                                    currentLightPath[currentRow, currentCol] = LightDown;
                                    break;
                                case VerticalSplitter:
                                    if (haveWeBeenHereBefore)
                                    {
                                        if (HaveWeLooped(currentCol, currentRow, currentLightPath, LightUp))
                                        {
                                            lightNotExitedOrLooped = false;
                                            continue;
                                        }
                                    }
                                    currentLightPath[currentRow, currentCol] = LightUp;
                                    startPoints.Add(new StartPoint {
                                        Row = currentRow,
                                        Column = currentCol,
                                        LightDirection = LightDown
                                    });
                                    break;
                            }
                        }
                        break;
                    case LightRight:
                        if (currentCol == columns - 1)
                        {
                            lightNotExitedOrLooped = false;
                            continue;
                        }
                        else
                        {
                            currentCol += 1;
                            var nextCell = mirrorGrid[currentRow, currentCol];
                            var haveWeBeenHereBefore = currentLightPath[currentRow, currentCol] != EmptySpace;
                            switch (nextCell)
                            {
                                case EmptySpace:
                                case HorizontalSplitter:
                                    if (haveWeBeenHereBefore)
                                    {
                                        if (HaveWeLooped(currentCol, currentRow, currentLightPath, LightRight))
                                        {
                                            lightNotExitedOrLooped = false;
                                            continue;
                                        }
                                    }
                                    currentLightPath[currentRow, currentCol] = LightRight;
                                    break;
                                case LeftMirror:
                                    if (haveWeBeenHereBefore)
                                    {
                                        if (HaveWeLooped(currentCol, currentRow, currentLightPath, LightDown))
                                        {
                                            lightNotExitedOrLooped = false;
                                            continue;
                                        }
                                    }
                                    currentLightPath[currentRow, currentCol] = LightDown;
                                    break;
                                case RightMirror:
                                    if (haveWeBeenHereBefore)
                                    {
                                        if (HaveWeLooped(currentCol, currentRow, currentLightPath, LightUp))
                                        {
                                            lightNotExitedOrLooped = false;
                                            continue;
                                        }
                                    }
                                    currentLightPath[currentRow, currentCol] = LightUp;
                                    break;
                                case VerticalSplitter:
                                    if (haveWeBeenHereBefore)
                                    {
                                        if (HaveWeLooped(currentCol, currentRow, currentLightPath, LightUp))
                                        {
                                            lightNotExitedOrLooped = false;
                                            continue;
                                        }
                                    }
                                    currentLightPath[currentRow, currentCol] = LightUp;
                                    startPoints.Add(new StartPoint {
                                        Row = currentRow,
                                        Column = currentCol,
                                        LightDirection = LightDown
                                    });
                                    break;
                            }
                        }
                        break;
                }
            }

            return startPoints;
        }

        private static bool HaveWeLooped(int currentCol, int currentRow, char[,] currentLightPath, char symbolToCheck)
        {
            var isLooped = false;
            if (currentLightPath[currentRow, currentCol] == symbolToCheck) {
                isLooped = true;
            }

            return isLooped;
        }

        public string SolveB ()
        {
            var rows = Lines.Length;
            var columns = Lines.First().Length;

            var lightGrid = new char[rows, columns];
            var mirrorGrid = new char[rows, columns];
            for (int i = 0; i < Lines.Length; i++)
            {
                var chars = Lines[i].ToCharArray();
                for (int j = 0; j < chars.Length; j++)
                {
                    mirrorGrid[i, j] = chars[j];
                    lightGrid[i, j] = EmptySpace;
                }
            }

            var biggestCount = 0;
            for (int i = 0; i < columns; i++) {
                var initalDirection = LightDown;
                var isSplit = false;
                switch (mirrorGrid[0, i])
                {
                    case EmptySpace:
                    case VerticalSplitter:
                        break;
                    case LeftMirror:
                        initalDirection = LightRight;
                        break;
                    case HorizontalSplitter:
                        initalDirection = LightLeft;
                        isSplit = true;
                        break;
                    case RightMirror:
                        initalDirection = LightLeft;
                        break;
                }

                var startPoint = new StartPoint { Row = 0, Column = i, LightDirection = initalDirection };
                var startPoints = new List<StartPoint>() { startPoint };
                if (isSplit) {
                    startPoints.Add(new StartPoint{ Row = 0, Column = i, LightDirection = LightRight });
                }

                var nextCount = NumberOfTilesEnergised(rows, columns, lightGrid, mirrorGrid, startPoints);
                biggestCount = Math.Max(nextCount, biggestCount);
            }

            for (int i = 0; i < rows; i++) {
                var initalDirection = LightRight;
                var isSplit = false;
                switch (mirrorGrid[i, 0])
                {
                    case EmptySpace:
                    case HorizontalSplitter:
                        break;
                    case LeftMirror:
                        initalDirection = LightDown;
                        break;
                    case VerticalSplitter:
                        initalDirection = LightUp;
                        isSplit = true;
                        break;
                    case RightMirror:
                        initalDirection = LightUp;
                        break;
                }

                var startPoint = new StartPoint { Row = i, Column = 0, LightDirection = initalDirection };
                var startPoints = new List<StartPoint>() { startPoint };
                if (isSplit) {
                    startPoints.Add(new StartPoint{ Row = i, Column = 0, LightDirection = LightDown });
                }

                var nextCount = NumberOfTilesEnergised(rows, columns, lightGrid, mirrorGrid, startPoints);
                biggestCount = Math.Max(nextCount, biggestCount);
            }

            for (int i = 0; i < columns; i++) {
                var initalDirection = LightUp;
                var isSplit = false;
                switch (mirrorGrid[rows - 1, i])
                {
                    case EmptySpace:
                    case VerticalSplitter:
                        break;
                    case LeftMirror:
                        initalDirection = LightLeft;
                        break;
                    case HorizontalSplitter:
                        initalDirection = LightRight;
                        isSplit = true;
                        break;
                    case RightMirror:
                        initalDirection = LightRight;
                        break;
                }

                var startPoint = new StartPoint { Row = rows - 1, Column = i, LightDirection = initalDirection };
                var startPoints = new List<StartPoint>() { startPoint };
                if (isSplit) {
                    startPoints.Add(new StartPoint{ Row = rows - 1, Column = i, LightDirection = LightLeft });
                }

                var nextCount = NumberOfTilesEnergised(rows, columns, lightGrid, mirrorGrid, startPoints);
                biggestCount = Math.Max(nextCount, biggestCount);
            }

            for (int i = 0; i < rows; i++) {
                var initalDirection = LightLeft;
                var isSplit = false;
                switch (mirrorGrid[i, columns - 1])
                {
                    case EmptySpace:
                    case HorizontalSplitter:
                        break;
                    case LeftMirror:
                        initalDirection = LightUp;
                        break;
                    case VerticalSplitter:
                        initalDirection = LightDown;
                        isSplit = true;
                        break;
                    case RightMirror:
                        initalDirection = LightDown;
                        break;
                }

                var startPoint = new StartPoint { Row = i, Column = columns - 1, LightDirection = initalDirection };
                var startPoints = new List<StartPoint>() { startPoint };
                if (isSplit) {
                    startPoints.Add(new StartPoint{ Row = i, Column = columns - 1, LightDirection = LightUp });
                }

                var nextCount = NumberOfTilesEnergised(rows, columns, lightGrid, mirrorGrid, startPoints);
                biggestCount = Math.Max(nextCount, biggestCount);
            }

            return biggestCount.ToString();
        }

        private int NumberOfTilesEnergised(int rows, int columns, char[,] lightGrid, char[,] mirrorGrid, List<StartPoint> startingPoints)
        {
            var lightPaths = new List<char[,]>();
            var startPoints = new List<StartPoint>();

            //var newStartPoints = WorkOutLightPath(rows, columns, mirrorGrid, firstLightPath, startPoint);
            CheckNewStartPointsAndResolve(rows, columns, lightGrid, mirrorGrid, startPoints, startingPoints, lightPaths);

            var count = 0;
            for (int i = 0; i < Lines.Length; i++)
            {
                var chars = Lines[i].ToCharArray();
                for (int j = 0; j < chars.Length; j++)
                {
                    if (lightPaths.Any(x => x[i, j] != '.'))
                    {
                        count += 1;
                    }
                }
            }

            return count;
        }
    }

    public class StartPoint {
        public int Row { get; set; }

        public int Column { get; set; }

        public char LightDirection { get; set; }
    }
}