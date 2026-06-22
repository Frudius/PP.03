namespace ReviewSamples.Modules.Variants;

public class Variant07_ShowtimeFixed
{
    public string Movie { get; set; }
    public AgeRating Rating { get; set; }
    public DateTime Time { get; set; }
    public bool[,] Seats { get; set; }
}

public enum AgeRating { G, PG, PG13, R, NC17 }
public enum TicketCategory { Child, Student, Pensioner, Adult }

public class Variant07_BoxOfficeFixed
{
    private const decimal BasePrice = 300m;
    private const decimal ChildDiscount = 0.5m;
    private const decimal StudentDiscount = 0.7m;
    private const decimal PensionerDiscount = 0.6m;
    private const decimal MorningDiscount = 0.8m;
    private const int MorningHourLimit = 12;

    public Ticket PurchaseTicket(Variant07_ShowtimeFixed showtime, int row, int seat, TicketCategory category, int viewerAge)
    {
        ArgumentNullException.ThrowIfNull(showtime);

        if (row < 0 || row >= showtime.Seats.GetLength(0))
            throw new ArgumentOutOfRangeException(nameof(row), "Некорректный номер ряда");

        if (seat < 0 || seat >= showtime.Seats.GetLength(1))
            throw new ArgumentOutOfRangeException(nameof(seat), "Некорректный номер места");

        if (viewerAge < 0)
            throw new ArgumentException("Возраст не может быть отрицательным", nameof(viewerAge));

        if (showtime.Seats[row, seat])
            throw new InvalidOperationException("Место уже занято");

        if (showtime.Rating == AgeRating.R && viewerAge < 18)
            throw new InvalidOperationException("Фильм имеет рейтинг 18+, ваш возраст не подходит");

        decimal price = BasePrice;
        price *= GetCategoryDiscount(category);
        if (showtime.Time.Hour < MorningHourLimit)
            price *= MorningDiscount;

        showtime.Seats[row, seat] = true;

        return new Ticket(showtime.Movie, row, seat, price);
    }

    private decimal GetCategoryDiscount(TicketCategory category) => category switch
    {
        TicketCategory.Child => ChildDiscount,
        TicketCategory.Student => StudentDiscount,
        TicketCategory.Pensioner => PensionerDiscount,
        _ => 1.0m
    };
}

public class Ticket
{
    public string Movie { get; }
    public int Row { get; }
    public int Seat { get; }
    public decimal Price { get; }

    public Ticket(string movie, int row, int seat, decimal price)
    {
        Movie = movie;
        Row = row;
        Seat = seat;
        Price = price;
    }
}