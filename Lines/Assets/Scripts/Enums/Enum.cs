public enum GameState
{
    StartGame,
    InGame,
    BallMoving,
    GameOver,
    Pausing
}
public enum BallState
{
    Queued,
    Ready,
    Selected,
    Moving,
    Stop,
    Destroy
}
public enum BallType
{
    Normal,
    Ghost,
    Diagonal,
    Bomb
}
public enum GameLevel
{
    Easy,
    Medium,
    Hard,
    Impossible,
    Custom
}