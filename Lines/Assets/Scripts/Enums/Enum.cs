public enum GameState
{
    StartGame,
    InGame,
    BallMoving,
    GameOver
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
