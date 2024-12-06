namespace rsmith985.AOC;


public class Line
{
    public Coord P1 {get; set;}
    public Coord P2 {get; set;}

    public Line(Coord p1, Coord p2)
    {
        this.P1 = p1;
        this.P2 = p2;
    }

    public double Slope{get; set;}
    public double Intercept{get;set;}
    public double Det{get;set;}
    public double DeltaX{get;set;}

    public void PreCalcValuesForLineIntersect()
    {
        this.Slope = (double)(this.P2.Y - this.P1.Y) / (this.P2.X - this.P1.X);
        this.Intercept = this.P2.Y - this.Slope*this.P2.X;
        
        this.Det = (double)this.P1.X * this.P2.Y - this.P1.Y * this.P2.X;
        this.DeltaX = this.P1.X - this.P2.X;
    }

    public (double X, double Y) GetIntersectionPointWith(Line l2)
    {
        var l1 = this;
        if(l1.Slope == l2.Slope) return (double.NaN, double.NaN);

        var x = (l1.Det * (l2.DeltaX) - l2.Det * (l1.DeltaX)) / (l1.Slope - l2.Slope);
        var y = l1.Slope * (x - l1.P1.X) + l1.P1.Y;

        return (x, y);
    }
}