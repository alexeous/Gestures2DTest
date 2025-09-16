namespace MathUtils.Curves;

/// <summary>
/// Абстрактная 2-мерная равномерно параметризованная кривая —
/// при изменении параметра t с постоянной скоростью точка на кривой будет двигаться тоже с постоянной скоростью.
/// Длина кривой всегда известна.
/// <seealso href="http://dfgm.math.msu.su/files/ivanov-tuzhilin/2017-2018/lecture6.pdf"/>
/// </summary>
public interface IUniformlyParameterizedCurve : ICurve
{
    float Length { get; }
}