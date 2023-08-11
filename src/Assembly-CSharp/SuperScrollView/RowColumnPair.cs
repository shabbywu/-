namespace SuperScrollView;

public struct RowColumnPair
{
	public int mRow;

	public int mColumn;

	public RowColumnPair(int row1, int column1)
	{
		mRow = row1;
		mColumn = column1;
	}

	public bool Equals(RowColumnPair other)
	{
		if (mRow == other.mRow)
		{
			return mColumn == other.mColumn;
		}
		return false;
	}

	public static bool operator ==(RowColumnPair a, RowColumnPair b)
	{
		if (a.mRow == b.mRow)
		{
			return a.mColumn == b.mColumn;
		}
		return false;
	}

	public static bool operator !=(RowColumnPair a, RowColumnPair b)
	{
		if (a.mRow == b.mRow)
		{
			return a.mColumn != b.mColumn;
		}
		return true;
	}

	public override int GetHashCode()
	{
		return 0;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (obj is RowColumnPair)
		{
			return Equals((RowColumnPair)obj);
		}
		return false;
	}
}
