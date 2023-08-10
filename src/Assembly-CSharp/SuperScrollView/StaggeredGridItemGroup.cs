using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperScrollView;

public class StaggeredGridItemGroup
{
	private LoopStaggeredGridView mParentGridView;

	private ListItemArrangeType mArrangeType;

	private List<LoopStaggeredGridViewItem> mItemList = new List<LoopStaggeredGridViewItem>();

	private RectTransform mContainerTrans;

	private ScrollRect mScrollRect;

	public int mGroupIndex;

	private GameObject mGameObject;

	private List<int> mItemIndexMap = new List<int>();

	private RectTransform mScrollRectTransform;

	private RectTransform mViewPortRectTransform;

	private float mItemDefaultWithPaddingSize;

	private int mItemTotalCount;

	private bool mIsVertList;

	private Func<int, int, LoopStaggeredGridViewItem> mOnGetItemByIndex;

	private Vector3[] mItemWorldCorners = (Vector3[])(object)new Vector3[4];

	private Vector3[] mViewPortRectLocalCorners = (Vector3[])(object)new Vector3[4];

	private int mCurReadyMinItemIndex;

	private int mCurReadyMaxItemIndex;

	private bool mNeedCheckNextMinItem = true;

	private bool mNeedCheckNextMaxItem = true;

	private ItemPosMgr mItemPosMgr;

	private bool mSupportScrollBar = true;

	private int mLastItemIndex;

	private float mLastItemPadding;

	private Vector3 mLastFrameContainerPos = Vector3.zero;

	private int mListUpdateCheckFrameCount;

	public List<int> ItemIndexMap => mItemIndexMap;

	public float ViewPortSize
	{
		get
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			Rect rect;
			if (mIsVertList)
			{
				rect = mViewPortRectTransform.rect;
				return ((Rect)(ref rect)).height;
			}
			rect = mViewPortRectTransform.rect;
			return ((Rect)(ref rect)).width;
		}
	}

	public float ViewPortWidth
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Rect rect = mViewPortRectTransform.rect;
			return ((Rect)(ref rect)).width;
		}
	}

	public float ViewPortHeight
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Rect rect = mViewPortRectTransform.rect;
			return ((Rect)(ref rect)).height;
		}
	}

	private bool IsDraging => mParentGridView.IsDraging;

	public int HadCreatedItemCount => mItemIndexMap.Count;

	public void Init(LoopStaggeredGridView parent, int itemTotalCount, int groupIndex, Func<int, int, LoopStaggeredGridViewItem> onGetItemByIndex)
	{
		mGroupIndex = groupIndex;
		mParentGridView = parent;
		mArrangeType = mParentGridView.ArrangeType;
		mGameObject = ((Component)mParentGridView).gameObject;
		mScrollRect = mGameObject.GetComponent<ScrollRect>();
		mItemPosMgr = new ItemPosMgr(mItemDefaultWithPaddingSize);
		mScrollRectTransform = ((Component)mScrollRect).GetComponent<RectTransform>();
		mContainerTrans = mScrollRect.content;
		mViewPortRectTransform = mScrollRect.viewport;
		if ((Object)(object)mViewPortRectTransform == (Object)null)
		{
			mViewPortRectTransform = mScrollRectTransform;
		}
		mIsVertList = mArrangeType == ListItemArrangeType.TopToBottom || mArrangeType == ListItemArrangeType.BottomToTop;
		mOnGetItemByIndex = onGetItemByIndex;
		mItemTotalCount = itemTotalCount;
		mViewPortRectTransform.GetLocalCorners(mViewPortRectLocalCorners);
		if (mItemTotalCount < 0)
		{
			mSupportScrollBar = false;
		}
		if (mSupportScrollBar)
		{
			mItemPosMgr.SetItemMaxCount(mItemTotalCount);
		}
		else
		{
			mItemPosMgr.SetItemMaxCount(0);
		}
		mCurReadyMaxItemIndex = 0;
		mCurReadyMinItemIndex = 0;
		mNeedCheckNextMaxItem = true;
		mNeedCheckNextMinItem = true;
	}

	public void ResetListView()
	{
		mViewPortRectTransform.GetLocalCorners(mViewPortRectLocalCorners);
	}

	public LoopStaggeredGridViewItem GetShownItemByItemIndex(int itemIndex)
	{
		int count = mItemList.Count;
		if (count == 0)
		{
			return null;
		}
		if (itemIndex < mItemList[0].ItemIndex || itemIndex > mItemList[count - 1].ItemIndex)
		{
			return null;
		}
		for (int i = 0; i < count; i++)
		{
			LoopStaggeredGridViewItem loopStaggeredGridViewItem = mItemList[i];
			if (loopStaggeredGridViewItem.ItemIndex == itemIndex)
			{
				return loopStaggeredGridViewItem;
			}
		}
		return null;
	}

	public LoopStaggeredGridViewItem GetShownItemByIndexInGroup(int indexInGroup)
	{
		int count = mItemList.Count;
		if (count == 0)
		{
			return null;
		}
		if (indexInGroup < mItemList[0].ItemIndexInGroup || indexInGroup > mItemList[count - 1].ItemIndexInGroup)
		{
			return null;
		}
		int index = indexInGroup - mItemList[0].ItemIndexInGroup;
		return mItemList[index];
	}

	public int GetIndexInShownItemList(LoopStaggeredGridViewItem item)
	{
		if ((Object)(object)item == (Object)null)
		{
			return -1;
		}
		int count = mItemList.Count;
		if (count == 0)
		{
			return -1;
		}
		for (int i = 0; i < count; i++)
		{
			if ((Object)(object)mItemList[i] == (Object)(object)item)
			{
				return i;
			}
		}
		return -1;
	}

	public void RefreshAllShownItem()
	{
		if (mItemList.Count != 0)
		{
			RefreshAllShownItemWithFirstIndexInGroup(mItemList[0].ItemIndexInGroup);
		}
	}

	public void OnItemSizeChanged(int indexInGroup)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		LoopStaggeredGridViewItem shownItemByIndexInGroup = GetShownItemByIndexInGroup(indexInGroup);
		if ((Object)(object)shownItemByIndexInGroup == (Object)null)
		{
			return;
		}
		if (mSupportScrollBar)
		{
			Rect rect;
			if (mIsVertList)
			{
				rect = shownItemByIndexInGroup.CachedRectTransform.rect;
				SetItemSize(indexInGroup, ((Rect)(ref rect)).height, shownItemByIndexInGroup.Padding);
			}
			else
			{
				rect = shownItemByIndexInGroup.CachedRectTransform.rect;
				SetItemSize(indexInGroup, ((Rect)(ref rect)).width, shownItemByIndexInGroup.Padding);
			}
		}
		UpdateAllShownItemsPos();
	}

	public void RefreshItemByIndexInGroup(int indexInGroup)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		int count = mItemList.Count;
		if (count == 0 || indexInGroup < mItemList[0].ItemIndexInGroup || indexInGroup > mItemList[count - 1].ItemIndexInGroup)
		{
			return;
		}
		int itemIndexInGroup = mItemList[0].ItemIndexInGroup;
		int index = indexInGroup - itemIndexInGroup;
		LoopStaggeredGridViewItem loopStaggeredGridViewItem = mItemList[index];
		Vector3 anchoredPosition3D = loopStaggeredGridViewItem.CachedRectTransform.anchoredPosition3D;
		RecycleItemTmp(loopStaggeredGridViewItem);
		LoopStaggeredGridViewItem newItemByIndexInGroup = GetNewItemByIndexInGroup(indexInGroup);
		if ((Object)(object)newItemByIndexInGroup == (Object)null)
		{
			RefreshAllShownItemWithFirstIndexInGroup(itemIndexInGroup);
			return;
		}
		mItemList[index] = newItemByIndexInGroup;
		if (mIsVertList)
		{
			anchoredPosition3D.x = newItemByIndexInGroup.StartPosOffset;
		}
		else
		{
			anchoredPosition3D.y = newItemByIndexInGroup.StartPosOffset;
		}
		newItemByIndexInGroup.CachedRectTransform.anchoredPosition3D = anchoredPosition3D;
		OnItemSizeChanged(indexInGroup);
		ClearAllTmpRecycledItem();
	}

	public void RefreshAllShownItemWithFirstIndexInGroup(int firstItemIndexInGroup)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		int count = mItemList.Count;
		if (count == 0)
		{
			return;
		}
		Vector3 anchoredPosition3D = mItemList[0].CachedRectTransform.anchoredPosition3D;
		RecycleAllItem();
		for (int i = 0; i < count; i++)
		{
			int num = firstItemIndexInGroup + i;
			LoopStaggeredGridViewItem newItemByIndexInGroup = GetNewItemByIndexInGroup(num);
			if ((Object)(object)newItemByIndexInGroup == (Object)null)
			{
				break;
			}
			if (mIsVertList)
			{
				anchoredPosition3D.x = newItemByIndexInGroup.StartPosOffset;
			}
			else
			{
				anchoredPosition3D.y = newItemByIndexInGroup.StartPosOffset;
			}
			newItemByIndexInGroup.CachedRectTransform.anchoredPosition3D = anchoredPosition3D;
			if (mSupportScrollBar)
			{
				Rect rect;
				if (mIsVertList)
				{
					rect = newItemByIndexInGroup.CachedRectTransform.rect;
					SetItemSize(num, ((Rect)(ref rect)).height, newItemByIndexInGroup.Padding);
				}
				else
				{
					rect = newItemByIndexInGroup.CachedRectTransform.rect;
					SetItemSize(num, ((Rect)(ref rect)).width, newItemByIndexInGroup.Padding);
				}
			}
			mItemList.Add(newItemByIndexInGroup);
		}
		UpdateAllShownItemsPos();
		ClearAllTmpRecycledItem();
	}

	public void RefreshAllShownItemWithFirstIndexAndPos(int firstItemIndexInGroup, Vector3 pos)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		RecycleAllItem();
		LoopStaggeredGridViewItem newItemByIndexInGroup = GetNewItemByIndexInGroup(firstItemIndexInGroup);
		if ((Object)(object)newItemByIndexInGroup == (Object)null)
		{
			return;
		}
		if (mIsVertList)
		{
			pos.x = newItemByIndexInGroup.StartPosOffset;
		}
		else
		{
			pos.y = newItemByIndexInGroup.StartPosOffset;
		}
		newItemByIndexInGroup.CachedRectTransform.anchoredPosition3D = pos;
		if (mSupportScrollBar)
		{
			Rect rect;
			if (mIsVertList)
			{
				rect = newItemByIndexInGroup.CachedRectTransform.rect;
				SetItemSize(firstItemIndexInGroup, ((Rect)(ref rect)).height, newItemByIndexInGroup.Padding);
			}
			else
			{
				rect = newItemByIndexInGroup.CachedRectTransform.rect;
				SetItemSize(firstItemIndexInGroup, ((Rect)(ref rect)).width, newItemByIndexInGroup.Padding);
			}
		}
		mItemList.Add(newItemByIndexInGroup);
		UpdateAllShownItemsPos();
		mParentGridView.UpdateListViewWithDefault();
		ClearAllTmpRecycledItem();
	}

	private void SetItemSize(int itemIndex, float itemSize, float padding)
	{
		mItemPosMgr.SetItemSize(itemIndex, itemSize + padding);
		if (itemIndex >= mLastItemIndex)
		{
			mLastItemIndex = itemIndex;
			mLastItemPadding = padding;
		}
	}

	private bool GetPlusItemIndexAndPosAtGivenPos(float pos, ref int index, ref float itemPos)
	{
		return mItemPosMgr.GetItemIndexAndPosAtGivenPos(pos, ref index, ref itemPos);
	}

	public float GetItemPos(int itemIndex)
	{
		return mItemPosMgr.GetItemPos(itemIndex);
	}

	public Vector3 GetItemCornerPosInViewPort(LoopStaggeredGridViewItem item, ItemCornerEnum corner = ItemCornerEnum.LeftBottom)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		item.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
		return ((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[(int)corner]);
	}

	public void RecycleItemTmp(LoopStaggeredGridViewItem item)
	{
		mParentGridView.RecycleItemTmp(item);
	}

	public void RecycleAllItem()
	{
		foreach (LoopStaggeredGridViewItem mItem in mItemList)
		{
			RecycleItemTmp(mItem);
		}
		mItemList.Clear();
	}

	public void ClearAllTmpRecycledItem()
	{
		mParentGridView.ClearAllTmpRecycledItem();
	}

	private LoopStaggeredGridViewItem GetNewItemByIndexInGroup(int indexInGroup)
	{
		return mParentGridView.GetNewItemByGroupAndIndex(mGroupIndex, indexInGroup);
	}

	public void SetListItemCount(int itemCount)
	{
		if (itemCount == mItemTotalCount)
		{
			return;
		}
		int num = mItemTotalCount;
		mItemTotalCount = itemCount;
		UpdateItemIndexMap(num);
		if (num < mItemTotalCount)
		{
			mItemPosMgr.SetItemMaxCount(mItemTotalCount);
		}
		else
		{
			mItemPosMgr.SetItemMaxCount(HadCreatedItemCount);
			mItemPosMgr.SetItemMaxCount(mItemTotalCount);
		}
		RecycleAllItem();
		if (mItemTotalCount == 0)
		{
			mCurReadyMaxItemIndex = 0;
			mCurReadyMinItemIndex = 0;
			mNeedCheckNextMaxItem = false;
			mNeedCheckNextMinItem = false;
			mItemIndexMap.Clear();
		}
		else
		{
			if (mCurReadyMaxItemIndex >= mItemTotalCount)
			{
				mCurReadyMaxItemIndex = mItemTotalCount - 1;
			}
			mNeedCheckNextMaxItem = true;
			mNeedCheckNextMinItem = true;
		}
	}

	private void UpdateItemIndexMap(int oldItemTotalCount)
	{
		int count = mItemIndexMap.Count;
		if (count == 0)
		{
			return;
		}
		if (mItemTotalCount == 0)
		{
			mItemIndexMap.Clear();
		}
		else
		{
			if (mItemTotalCount >= oldItemTotalCount)
			{
				return;
			}
			int itemTotalCount = mParentGridView.ItemTotalCount;
			if (mItemIndexMap[count - 1] < itemTotalCount)
			{
				return;
			}
			int num = 0;
			int num2 = count - 1;
			int num3 = 0;
			while (num <= num2)
			{
				int num4 = (num + num2) / 2;
				int num5 = mItemIndexMap[num4];
				if (num5 == itemTotalCount)
				{
					num3 = num4;
					break;
				}
				if (num5 >= itemTotalCount)
				{
					break;
				}
				num = num4 + 1;
				num3 = num;
			}
			int num6 = 0;
			for (int i = num3; i < count; i++)
			{
				if (mItemIndexMap[i] >= itemTotalCount)
				{
					num6 = i;
					break;
				}
			}
			mItemIndexMap.RemoveRange(num6, count - num6);
		}
	}

	public void UpdateListViewPart1(float distanceForRecycle0, float distanceForRecycle1, float distanceForNew0, float distanceForNew1)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		if (mSupportScrollBar)
		{
			mItemPosMgr.Update(updateAll: false);
		}
		mListUpdateCheckFrameCount = mParentGridView.ListUpdateCheckFrameCount;
		bool flag = true;
		int num = 0;
		int num2 = 9999;
		while (flag)
		{
			num++;
			if (num >= num2)
			{
				Debug.LogError((object)("UpdateListViewPart1 while loop " + num + " times! something is wrong!"));
				break;
			}
			flag = ((!mIsVertList) ? UpdateForHorizontalListPart1(distanceForRecycle0, distanceForRecycle1, distanceForNew0, distanceForNew1) : UpdateForVertListPart1(distanceForRecycle0, distanceForRecycle1, distanceForNew0, distanceForNew1));
		}
		mLastFrameContainerPos = mContainerTrans.anchoredPosition3D;
	}

	public bool UpdateListViewPart2(float distanceForRecycle0, float distanceForRecycle1, float distanceForNew0, float distanceForNew1)
	{
		if (mIsVertList)
		{
			return UpdateForVertListPart2(distanceForRecycle0, distanceForRecycle1, distanceForNew0, distanceForNew1);
		}
		return UpdateForHorizontalListPart2(distanceForRecycle0, distanceForRecycle1, distanceForNew0, distanceForNew1);
	}

	public bool UpdateForVertListPart1(float distanceForRecycle0, float distanceForRecycle1, float distanceForNew0, float distanceForNew1)
	{
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_067f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_078a: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0900: Unknown result type (might be due to invalid IL or missing references)
		//IL_0911: Unknown result type (might be due to invalid IL or missing references)
		//IL_0916: Unknown result type (might be due to invalid IL or missing references)
		//IL_093f: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		if (mItemTotalCount == 0)
		{
			if (mItemList.Count > 0)
			{
				RecycleAllItem();
			}
			return false;
		}
		Rect rect;
		if (mArrangeType == ListItemArrangeType.TopToBottom)
		{
			if (mItemList.Count == 0)
			{
				float num = mContainerTrans.anchoredPosition3D.y;
				if (num < 0f)
				{
					num = 0f;
				}
				int index = 0;
				float itemPos = 0f - num;
				if (mSupportScrollBar)
				{
					if (!GetPlusItemIndexAndPosAtGivenPos(num, ref index, ref itemPos))
					{
						return false;
					}
					itemPos = 0f - itemPos;
				}
				LoopStaggeredGridViewItem newItemByIndexInGroup = GetNewItemByIndexInGroup(index);
				if ((Object)(object)newItemByIndexInGroup == (Object)null)
				{
					return false;
				}
				if (mSupportScrollBar)
				{
					int itemIndex = index;
					rect = newItemByIndexInGroup.CachedRectTransform.rect;
					SetItemSize(itemIndex, ((Rect)(ref rect)).height, newItemByIndexInGroup.Padding);
				}
				mItemList.Add(newItemByIndexInGroup);
				newItemByIndexInGroup.CachedRectTransform.anchoredPosition3D = new Vector3(newItemByIndexInGroup.StartPosOffset, itemPos, 0f);
				return true;
			}
			LoopStaggeredGridViewItem loopStaggeredGridViewItem = mItemList[0];
			loopStaggeredGridViewItem.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
			Vector3 val = ((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[1]);
			Vector3 val2 = ((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[0]);
			if (!IsDraging && loopStaggeredGridViewItem.ItemCreatedCheckFrameCount != mListUpdateCheckFrameCount && val2.y - mViewPortRectLocalCorners[1].y > distanceForRecycle0)
			{
				mItemList.RemoveAt(0);
				RecycleItemTmp(loopStaggeredGridViewItem);
				if (!mSupportScrollBar)
				{
					CheckIfNeedUpdateItemPos();
				}
				return true;
			}
			LoopStaggeredGridViewItem loopStaggeredGridViewItem2 = mItemList[mItemList.Count - 1];
			loopStaggeredGridViewItem2.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
			Vector3 val3 = ((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[1]);
			Vector3 val4 = ((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[0]);
			if (!IsDraging && loopStaggeredGridViewItem2.ItemCreatedCheckFrameCount != mListUpdateCheckFrameCount && mViewPortRectLocalCorners[0].y - val3.y > distanceForRecycle1)
			{
				mItemList.RemoveAt(mItemList.Count - 1);
				RecycleItemTmp(loopStaggeredGridViewItem2);
				if (!mSupportScrollBar)
				{
					CheckIfNeedUpdateItemPos();
				}
				return true;
			}
			if (val.y - mViewPortRectLocalCorners[1].y < distanceForNew0)
			{
				if (loopStaggeredGridViewItem.ItemIndexInGroup < mCurReadyMinItemIndex)
				{
					mCurReadyMinItemIndex = loopStaggeredGridViewItem.ItemIndexInGroup;
					mNeedCheckNextMinItem = true;
				}
				int num2 = loopStaggeredGridViewItem.ItemIndexInGroup - 1;
				if (num2 >= mCurReadyMinItemIndex || mNeedCheckNextMinItem)
				{
					LoopStaggeredGridViewItem newItemByIndexInGroup2 = GetNewItemByIndexInGroup(num2);
					if (!((Object)(object)newItemByIndexInGroup2 == (Object)null))
					{
						if (mSupportScrollBar)
						{
							rect = newItemByIndexInGroup2.CachedRectTransform.rect;
							SetItemSize(num2, ((Rect)(ref rect)).height, newItemByIndexInGroup2.Padding);
						}
						mItemList.Insert(0, newItemByIndexInGroup2);
						float y = loopStaggeredGridViewItem.CachedRectTransform.anchoredPosition3D.y;
						rect = newItemByIndexInGroup2.CachedRectTransform.rect;
						float num3 = y + ((Rect)(ref rect)).height + newItemByIndexInGroup2.Padding;
						newItemByIndexInGroup2.CachedRectTransform.anchoredPosition3D = new Vector3(newItemByIndexInGroup2.StartPosOffset, num3, 0f);
						CheckIfNeedUpdateItemPos();
						if (num2 < mCurReadyMinItemIndex)
						{
							mCurReadyMinItemIndex = num2;
						}
						return true;
					}
					mCurReadyMinItemIndex = loopStaggeredGridViewItem.ItemIndexInGroup;
					mNeedCheckNextMinItem = false;
				}
			}
			if (mViewPortRectLocalCorners[0].y - val4.y < distanceForNew1)
			{
				if (loopStaggeredGridViewItem2.ItemIndexInGroup > mCurReadyMaxItemIndex)
				{
					mCurReadyMaxItemIndex = loopStaggeredGridViewItem2.ItemIndexInGroup;
					mNeedCheckNextMaxItem = true;
				}
				int num4 = loopStaggeredGridViewItem2.ItemIndexInGroup + 1;
				if (num4 >= mItemIndexMap.Count)
				{
					return false;
				}
				if (num4 <= mCurReadyMaxItemIndex || mNeedCheckNextMaxItem)
				{
					LoopStaggeredGridViewItem newItemByIndexInGroup3 = GetNewItemByIndexInGroup(num4);
					if ((Object)(object)newItemByIndexInGroup3 == (Object)null)
					{
						mCurReadyMaxItemIndex = loopStaggeredGridViewItem2.ItemIndexInGroup;
						mNeedCheckNextMaxItem = false;
						CheckIfNeedUpdateItemPos();
						return false;
					}
					if (mSupportScrollBar)
					{
						rect = newItemByIndexInGroup3.CachedRectTransform.rect;
						SetItemSize(num4, ((Rect)(ref rect)).height, newItemByIndexInGroup3.Padding);
					}
					mItemList.Add(newItemByIndexInGroup3);
					float y2 = loopStaggeredGridViewItem2.CachedRectTransform.anchoredPosition3D.y;
					rect = loopStaggeredGridViewItem2.CachedRectTransform.rect;
					float num5 = y2 - ((Rect)(ref rect)).height - loopStaggeredGridViewItem2.Padding;
					newItemByIndexInGroup3.CachedRectTransform.anchoredPosition3D = new Vector3(newItemByIndexInGroup3.StartPosOffset, num5, 0f);
					CheckIfNeedUpdateItemPos();
					if (num4 > mCurReadyMaxItemIndex)
					{
						mCurReadyMaxItemIndex = num4;
					}
					return true;
				}
			}
		}
		else
		{
			if (mItemList.Count == 0)
			{
				float num6 = mContainerTrans.anchoredPosition3D.y;
				if (num6 > 0f)
				{
					num6 = 0f;
				}
				int index2 = 0;
				float itemPos2 = 0f - num6;
				if (mSupportScrollBar && !GetPlusItemIndexAndPosAtGivenPos(0f - num6, ref index2, ref itemPos2))
				{
					return false;
				}
				LoopStaggeredGridViewItem newItemByIndexInGroup4 = GetNewItemByIndexInGroup(index2);
				if ((Object)(object)newItemByIndexInGroup4 == (Object)null)
				{
					return false;
				}
				if (mSupportScrollBar)
				{
					int itemIndex2 = index2;
					rect = newItemByIndexInGroup4.CachedRectTransform.rect;
					SetItemSize(itemIndex2, ((Rect)(ref rect)).height, newItemByIndexInGroup4.Padding);
				}
				mItemList.Add(newItemByIndexInGroup4);
				newItemByIndexInGroup4.CachedRectTransform.anchoredPosition3D = new Vector3(newItemByIndexInGroup4.StartPosOffset, itemPos2, 0f);
				return true;
			}
			LoopStaggeredGridViewItem loopStaggeredGridViewItem3 = mItemList[0];
			loopStaggeredGridViewItem3.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
			Vector3 val5 = ((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[1]);
			Vector3 val6 = ((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[0]);
			if (!IsDraging && loopStaggeredGridViewItem3.ItemCreatedCheckFrameCount != mListUpdateCheckFrameCount && mViewPortRectLocalCorners[0].y - val5.y > distanceForRecycle0)
			{
				mItemList.RemoveAt(0);
				RecycleItemTmp(loopStaggeredGridViewItem3);
				if (!mSupportScrollBar)
				{
					CheckIfNeedUpdateItemPos();
				}
				return true;
			}
			LoopStaggeredGridViewItem loopStaggeredGridViewItem4 = mItemList[mItemList.Count - 1];
			loopStaggeredGridViewItem4.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
			Vector3 val7 = ((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[1]);
			Vector3 val8 = ((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[0]);
			if (!IsDraging && loopStaggeredGridViewItem4.ItemCreatedCheckFrameCount != mListUpdateCheckFrameCount && val8.y - mViewPortRectLocalCorners[1].y > distanceForRecycle1)
			{
				mItemList.RemoveAt(mItemList.Count - 1);
				RecycleItemTmp(loopStaggeredGridViewItem4);
				if (!mSupportScrollBar)
				{
					CheckIfNeedUpdateItemPos();
				}
				return true;
			}
			if (mViewPortRectLocalCorners[0].y - val6.y < distanceForNew0)
			{
				if (loopStaggeredGridViewItem3.ItemIndexInGroup < mCurReadyMinItemIndex)
				{
					mCurReadyMinItemIndex = loopStaggeredGridViewItem3.ItemIndexInGroup;
					mNeedCheckNextMinItem = true;
				}
				int num7 = loopStaggeredGridViewItem3.ItemIndexInGroup - 1;
				if (num7 >= mCurReadyMinItemIndex || mNeedCheckNextMinItem)
				{
					LoopStaggeredGridViewItem newItemByIndexInGroup5 = GetNewItemByIndexInGroup(num7);
					if (!((Object)(object)newItemByIndexInGroup5 == (Object)null))
					{
						if (mSupportScrollBar)
						{
							rect = newItemByIndexInGroup5.CachedRectTransform.rect;
							SetItemSize(num7, ((Rect)(ref rect)).height, newItemByIndexInGroup5.Padding);
						}
						mItemList.Insert(0, newItemByIndexInGroup5);
						float y3 = loopStaggeredGridViewItem3.CachedRectTransform.anchoredPosition3D.y;
						rect = newItemByIndexInGroup5.CachedRectTransform.rect;
						float num8 = y3 - ((Rect)(ref rect)).height - newItemByIndexInGroup5.Padding;
						newItemByIndexInGroup5.CachedRectTransform.anchoredPosition3D = new Vector3(newItemByIndexInGroup5.StartPosOffset, num8, 0f);
						CheckIfNeedUpdateItemPos();
						if (num7 < mCurReadyMinItemIndex)
						{
							mCurReadyMinItemIndex = num7;
						}
						return true;
					}
					mCurReadyMinItemIndex = loopStaggeredGridViewItem3.ItemIndexInGroup;
					mNeedCheckNextMinItem = false;
				}
			}
			if (val7.y - mViewPortRectLocalCorners[1].y < distanceForNew1)
			{
				if (loopStaggeredGridViewItem4.ItemIndexInGroup > mCurReadyMaxItemIndex)
				{
					mCurReadyMaxItemIndex = loopStaggeredGridViewItem4.ItemIndexInGroup;
					mNeedCheckNextMaxItem = true;
				}
				int num9 = loopStaggeredGridViewItem4.ItemIndexInGroup + 1;
				if (num9 >= mItemIndexMap.Count)
				{
					return false;
				}
				if (num9 <= mCurReadyMaxItemIndex || mNeedCheckNextMaxItem)
				{
					LoopStaggeredGridViewItem newItemByIndexInGroup6 = GetNewItemByIndexInGroup(num9);
					if ((Object)(object)newItemByIndexInGroup6 == (Object)null)
					{
						mCurReadyMaxItemIndex = loopStaggeredGridViewItem4.ItemIndexInGroup;
						mNeedCheckNextMaxItem = false;
						CheckIfNeedUpdateItemPos();
						return false;
					}
					if (mSupportScrollBar)
					{
						rect = newItemByIndexInGroup6.CachedRectTransform.rect;
						SetItemSize(num9, ((Rect)(ref rect)).height, newItemByIndexInGroup6.Padding);
					}
					mItemList.Add(newItemByIndexInGroup6);
					float y4 = loopStaggeredGridViewItem4.CachedRectTransform.anchoredPosition3D.y;
					rect = loopStaggeredGridViewItem4.CachedRectTransform.rect;
					float num10 = y4 + ((Rect)(ref rect)).height + loopStaggeredGridViewItem4.Padding;
					newItemByIndexInGroup6.CachedRectTransform.anchoredPosition3D = new Vector3(newItemByIndexInGroup6.StartPosOffset, num10, 0f);
					CheckIfNeedUpdateItemPos();
					if (num9 > mCurReadyMaxItemIndex)
					{
						mCurReadyMaxItemIndex = num9;
					}
					return true;
				}
			}
		}
		return false;
	}

	public bool UpdateForVertListPart2(float distanceForRecycle0, float distanceForRecycle1, float distanceForNew0, float distanceForNew1)
	{
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		if (mItemTotalCount == 0)
		{
			if (mItemList.Count > 0)
			{
				RecycleAllItem();
			}
			return false;
		}
		Rect rect;
		if (mArrangeType == ListItemArrangeType.TopToBottom)
		{
			if (mItemList.Count == 0)
			{
				float num = mContainerTrans.anchoredPosition3D.y;
				if (num < 0f)
				{
					num = 0f;
				}
				int index = 0;
				float itemPos = 0f - num;
				if (mSupportScrollBar)
				{
					if (!GetPlusItemIndexAndPosAtGivenPos(num, ref index, ref itemPos))
					{
						return false;
					}
					itemPos = 0f - itemPos;
				}
				LoopStaggeredGridViewItem newItemByIndexInGroup = GetNewItemByIndexInGroup(index);
				if ((Object)(object)newItemByIndexInGroup == (Object)null)
				{
					return false;
				}
				if (mSupportScrollBar)
				{
					int itemIndex = index;
					rect = newItemByIndexInGroup.CachedRectTransform.rect;
					SetItemSize(itemIndex, ((Rect)(ref rect)).height, newItemByIndexInGroup.Padding);
				}
				mItemList.Add(newItemByIndexInGroup);
				newItemByIndexInGroup.CachedRectTransform.anchoredPosition3D = new Vector3(newItemByIndexInGroup.StartPosOffset, itemPos, 0f);
				return true;
			}
			LoopStaggeredGridViewItem loopStaggeredGridViewItem = mItemList[mItemList.Count - 1];
			loopStaggeredGridViewItem.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
			Vector3 val = ((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[0]);
			if (mViewPortRectLocalCorners[0].y - val.y < distanceForNew1)
			{
				if (loopStaggeredGridViewItem.ItemIndexInGroup > mCurReadyMaxItemIndex)
				{
					mCurReadyMaxItemIndex = loopStaggeredGridViewItem.ItemIndexInGroup;
					mNeedCheckNextMaxItem = true;
				}
				int num2 = loopStaggeredGridViewItem.ItemIndexInGroup + 1;
				if (num2 <= mCurReadyMaxItemIndex || mNeedCheckNextMaxItem)
				{
					LoopStaggeredGridViewItem newItemByIndexInGroup2 = GetNewItemByIndexInGroup(num2);
					if ((Object)(object)newItemByIndexInGroup2 == (Object)null)
					{
						mCurReadyMaxItemIndex = loopStaggeredGridViewItem.ItemIndexInGroup;
						mNeedCheckNextMaxItem = false;
						CheckIfNeedUpdateItemPos();
						return false;
					}
					if (mSupportScrollBar)
					{
						rect = newItemByIndexInGroup2.CachedRectTransform.rect;
						SetItemSize(num2, ((Rect)(ref rect)).height, newItemByIndexInGroup2.Padding);
					}
					mItemList.Add(newItemByIndexInGroup2);
					float y = loopStaggeredGridViewItem.CachedRectTransform.anchoredPosition3D.y;
					rect = loopStaggeredGridViewItem.CachedRectTransform.rect;
					float num3 = y - ((Rect)(ref rect)).height - loopStaggeredGridViewItem.Padding;
					newItemByIndexInGroup2.CachedRectTransform.anchoredPosition3D = new Vector3(newItemByIndexInGroup2.StartPosOffset, num3, 0f);
					CheckIfNeedUpdateItemPos();
					if (num2 > mCurReadyMaxItemIndex)
					{
						mCurReadyMaxItemIndex = num2;
					}
					return true;
				}
			}
		}
		else
		{
			if (mItemList.Count == 0)
			{
				float num4 = mContainerTrans.anchoredPosition3D.y;
				if (num4 > 0f)
				{
					num4 = 0f;
				}
				int index2 = 0;
				float itemPos2 = 0f - num4;
				if (mSupportScrollBar && !GetPlusItemIndexAndPosAtGivenPos(0f - num4, ref index2, ref itemPos2))
				{
					return false;
				}
				LoopStaggeredGridViewItem newItemByIndexInGroup3 = GetNewItemByIndexInGroup(index2);
				if ((Object)(object)newItemByIndexInGroup3 == (Object)null)
				{
					return false;
				}
				if (mSupportScrollBar)
				{
					int itemIndex2 = index2;
					rect = newItemByIndexInGroup3.CachedRectTransform.rect;
					SetItemSize(itemIndex2, ((Rect)(ref rect)).height, newItemByIndexInGroup3.Padding);
				}
				mItemList.Add(newItemByIndexInGroup3);
				newItemByIndexInGroup3.CachedRectTransform.anchoredPosition3D = new Vector3(newItemByIndexInGroup3.StartPosOffset, itemPos2, 0f);
				return true;
			}
			LoopStaggeredGridViewItem loopStaggeredGridViewItem2 = mItemList[mItemList.Count - 1];
			loopStaggeredGridViewItem2.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
			if (((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[1]).y - mViewPortRectLocalCorners[1].y < distanceForNew1)
			{
				if (loopStaggeredGridViewItem2.ItemIndexInGroup > mCurReadyMaxItemIndex)
				{
					mCurReadyMaxItemIndex = loopStaggeredGridViewItem2.ItemIndexInGroup;
					mNeedCheckNextMaxItem = true;
				}
				int num5 = loopStaggeredGridViewItem2.ItemIndexInGroup + 1;
				if (num5 <= mCurReadyMaxItemIndex || mNeedCheckNextMaxItem)
				{
					LoopStaggeredGridViewItem newItemByIndexInGroup4 = GetNewItemByIndexInGroup(num5);
					if ((Object)(object)newItemByIndexInGroup4 == (Object)null)
					{
						mCurReadyMaxItemIndex = loopStaggeredGridViewItem2.ItemIndexInGroup;
						mNeedCheckNextMaxItem = false;
						CheckIfNeedUpdateItemPos();
						return false;
					}
					if (mSupportScrollBar)
					{
						rect = newItemByIndexInGroup4.CachedRectTransform.rect;
						SetItemSize(num5, ((Rect)(ref rect)).height, newItemByIndexInGroup4.Padding);
					}
					mItemList.Add(newItemByIndexInGroup4);
					float y2 = loopStaggeredGridViewItem2.CachedRectTransform.anchoredPosition3D.y;
					rect = loopStaggeredGridViewItem2.CachedRectTransform.rect;
					float num6 = y2 + ((Rect)(ref rect)).height + loopStaggeredGridViewItem2.Padding;
					newItemByIndexInGroup4.CachedRectTransform.anchoredPosition3D = new Vector3(newItemByIndexInGroup4.StartPosOffset, num6, 0f);
					CheckIfNeedUpdateItemPos();
					if (num5 > mCurReadyMaxItemIndex)
					{
						mCurReadyMaxItemIndex = num5;
					}
					return true;
				}
			}
		}
		return false;
	}

	public bool UpdateForHorizontalListPart1(float distanceForRecycle0, float distanceForRecycle1, float distanceForNew0, float distanceForNew1)
	{
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_066f: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0793: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0907: Unknown result type (might be due to invalid IL or missing references)
		//IL_0918: Unknown result type (might be due to invalid IL or missing references)
		//IL_091d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0946: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08de: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		if (mItemTotalCount == 0)
		{
			if (mItemList.Count > 0)
			{
				RecycleAllItem();
			}
			return false;
		}
		Rect rect;
		if (mArrangeType == ListItemArrangeType.LeftToRight)
		{
			if (mItemList.Count == 0)
			{
				float num = mContainerTrans.anchoredPosition3D.x;
				if (num > 0f)
				{
					num = 0f;
				}
				int index = 0;
				float itemPos = 0f - num;
				if (mSupportScrollBar && !GetPlusItemIndexAndPosAtGivenPos(0f - num, ref index, ref itemPos))
				{
					return false;
				}
				LoopStaggeredGridViewItem newItemByIndexInGroup = GetNewItemByIndexInGroup(index);
				if ((Object)(object)newItemByIndexInGroup == (Object)null)
				{
					return false;
				}
				if (mSupportScrollBar)
				{
					int itemIndex = index;
					rect = newItemByIndexInGroup.CachedRectTransform.rect;
					SetItemSize(itemIndex, ((Rect)(ref rect)).width, newItemByIndexInGroup.Padding);
				}
				mItemList.Add(newItemByIndexInGroup);
				newItemByIndexInGroup.CachedRectTransform.anchoredPosition3D = new Vector3(itemPos, newItemByIndexInGroup.StartPosOffset, 0f);
				return true;
			}
			LoopStaggeredGridViewItem loopStaggeredGridViewItem = mItemList[0];
			loopStaggeredGridViewItem.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
			Vector3 val = ((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[1]);
			Vector3 val2 = ((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[2]);
			if (!IsDraging && loopStaggeredGridViewItem.ItemCreatedCheckFrameCount != mListUpdateCheckFrameCount && mViewPortRectLocalCorners[1].x - val2.x > distanceForRecycle0)
			{
				mItemList.RemoveAt(0);
				RecycleItemTmp(loopStaggeredGridViewItem);
				if (!mSupportScrollBar)
				{
					CheckIfNeedUpdateItemPos();
				}
				return true;
			}
			LoopStaggeredGridViewItem loopStaggeredGridViewItem2 = mItemList[mItemList.Count - 1];
			loopStaggeredGridViewItem2.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
			Vector3 val3 = ((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[1]);
			Vector3 val4 = ((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[2]);
			if (!IsDraging && loopStaggeredGridViewItem2.ItemCreatedCheckFrameCount != mListUpdateCheckFrameCount && val3.x - mViewPortRectLocalCorners[2].x > distanceForRecycle1)
			{
				mItemList.RemoveAt(mItemList.Count - 1);
				RecycleItemTmp(loopStaggeredGridViewItem2);
				if (!mSupportScrollBar)
				{
					CheckIfNeedUpdateItemPos();
				}
				return true;
			}
			if (mViewPortRectLocalCorners[1].x - val.x < distanceForNew0)
			{
				if (loopStaggeredGridViewItem.ItemIndexInGroup < mCurReadyMinItemIndex)
				{
					mCurReadyMinItemIndex = loopStaggeredGridViewItem.ItemIndexInGroup;
					mNeedCheckNextMinItem = true;
				}
				int num2 = loopStaggeredGridViewItem.ItemIndexInGroup - 1;
				if (num2 >= mCurReadyMinItemIndex || mNeedCheckNextMinItem)
				{
					LoopStaggeredGridViewItem newItemByIndexInGroup2 = GetNewItemByIndexInGroup(num2);
					if (!((Object)(object)newItemByIndexInGroup2 == (Object)null))
					{
						if (mSupportScrollBar)
						{
							rect = newItemByIndexInGroup2.CachedRectTransform.rect;
							SetItemSize(num2, ((Rect)(ref rect)).width, newItemByIndexInGroup2.Padding);
						}
						mItemList.Insert(0, newItemByIndexInGroup2);
						float x = loopStaggeredGridViewItem.CachedRectTransform.anchoredPosition3D.x;
						rect = newItemByIndexInGroup2.CachedRectTransform.rect;
						float num3 = x - ((Rect)(ref rect)).width - newItemByIndexInGroup2.Padding;
						newItemByIndexInGroup2.CachedRectTransform.anchoredPosition3D = new Vector3(num3, newItemByIndexInGroup2.StartPosOffset, 0f);
						CheckIfNeedUpdateItemPos();
						if (num2 < mCurReadyMinItemIndex)
						{
							mCurReadyMinItemIndex = num2;
						}
						return true;
					}
					mCurReadyMinItemIndex = loopStaggeredGridViewItem.ItemIndexInGroup;
					mNeedCheckNextMinItem = false;
				}
			}
			if (val4.x - mViewPortRectLocalCorners[2].x < distanceForNew1)
			{
				if (loopStaggeredGridViewItem2.ItemIndexInGroup > mCurReadyMaxItemIndex)
				{
					mCurReadyMaxItemIndex = loopStaggeredGridViewItem2.ItemIndexInGroup;
					mNeedCheckNextMaxItem = true;
				}
				int num4 = loopStaggeredGridViewItem2.ItemIndexInGroup + 1;
				if (num4 >= mItemIndexMap.Count)
				{
					return false;
				}
				if (num4 <= mCurReadyMaxItemIndex || mNeedCheckNextMaxItem)
				{
					LoopStaggeredGridViewItem newItemByIndexInGroup3 = GetNewItemByIndexInGroup(num4);
					if (!((Object)(object)newItemByIndexInGroup3 == (Object)null))
					{
						if (mSupportScrollBar)
						{
							rect = newItemByIndexInGroup3.CachedRectTransform.rect;
							SetItemSize(num4, ((Rect)(ref rect)).width, newItemByIndexInGroup3.Padding);
						}
						mItemList.Add(newItemByIndexInGroup3);
						float x2 = loopStaggeredGridViewItem2.CachedRectTransform.anchoredPosition3D.x;
						rect = loopStaggeredGridViewItem2.CachedRectTransform.rect;
						float num5 = x2 + ((Rect)(ref rect)).width + loopStaggeredGridViewItem2.Padding;
						newItemByIndexInGroup3.CachedRectTransform.anchoredPosition3D = new Vector3(num5, newItemByIndexInGroup3.StartPosOffset, 0f);
						CheckIfNeedUpdateItemPos();
						if (num4 > mCurReadyMaxItemIndex)
						{
							mCurReadyMaxItemIndex = num4;
						}
						return true;
					}
					mCurReadyMaxItemIndex = loopStaggeredGridViewItem2.ItemIndexInGroup;
					mNeedCheckNextMaxItem = false;
					CheckIfNeedUpdateItemPos();
				}
			}
		}
		else
		{
			if (mItemList.Count == 0)
			{
				float num6 = mContainerTrans.anchoredPosition3D.x;
				if (num6 < 0f)
				{
					num6 = 0f;
				}
				int index2 = 0;
				float itemPos2 = 0f - num6;
				if (mSupportScrollBar)
				{
					if (!GetPlusItemIndexAndPosAtGivenPos(num6, ref index2, ref itemPos2))
					{
						return false;
					}
					itemPos2 = 0f - itemPos2;
				}
				LoopStaggeredGridViewItem newItemByIndexInGroup4 = GetNewItemByIndexInGroup(index2);
				if ((Object)(object)newItemByIndexInGroup4 == (Object)null)
				{
					return false;
				}
				if (mSupportScrollBar)
				{
					int itemIndex2 = index2;
					rect = newItemByIndexInGroup4.CachedRectTransform.rect;
					SetItemSize(itemIndex2, ((Rect)(ref rect)).width, newItemByIndexInGroup4.Padding);
				}
				mItemList.Add(newItemByIndexInGroup4);
				newItemByIndexInGroup4.CachedRectTransform.anchoredPosition3D = new Vector3(itemPos2, newItemByIndexInGroup4.StartPosOffset, 0f);
				return true;
			}
			LoopStaggeredGridViewItem loopStaggeredGridViewItem3 = mItemList[0];
			loopStaggeredGridViewItem3.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
			Vector3 val5 = ((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[1]);
			Vector3 val6 = ((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[2]);
			if (!IsDraging && loopStaggeredGridViewItem3.ItemCreatedCheckFrameCount != mListUpdateCheckFrameCount && val5.x - mViewPortRectLocalCorners[2].x > distanceForRecycle0)
			{
				mItemList.RemoveAt(0);
				RecycleItemTmp(loopStaggeredGridViewItem3);
				if (!mSupportScrollBar)
				{
					CheckIfNeedUpdateItemPos();
				}
				return true;
			}
			LoopStaggeredGridViewItem loopStaggeredGridViewItem4 = mItemList[mItemList.Count - 1];
			loopStaggeredGridViewItem4.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
			Vector3 val7 = ((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[1]);
			Vector3 val8 = ((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[2]);
			if (!IsDraging && loopStaggeredGridViewItem4.ItemCreatedCheckFrameCount != mListUpdateCheckFrameCount && mViewPortRectLocalCorners[1].x - val8.x > distanceForRecycle1)
			{
				mItemList.RemoveAt(mItemList.Count - 1);
				RecycleItemTmp(loopStaggeredGridViewItem4);
				if (!mSupportScrollBar)
				{
					CheckIfNeedUpdateItemPos();
				}
				return true;
			}
			if (val6.x - mViewPortRectLocalCorners[2].x < distanceForNew0)
			{
				if (loopStaggeredGridViewItem3.ItemIndexInGroup < mCurReadyMinItemIndex)
				{
					mCurReadyMinItemIndex = loopStaggeredGridViewItem3.ItemIndexInGroup;
					mNeedCheckNextMinItem = true;
				}
				int num7 = loopStaggeredGridViewItem3.ItemIndexInGroup - 1;
				if (num7 >= mCurReadyMinItemIndex || mNeedCheckNextMinItem)
				{
					LoopStaggeredGridViewItem newItemByIndexInGroup5 = GetNewItemByIndexInGroup(num7);
					if (!((Object)(object)newItemByIndexInGroup5 == (Object)null))
					{
						if (mSupportScrollBar)
						{
							rect = newItemByIndexInGroup5.CachedRectTransform.rect;
							SetItemSize(num7, ((Rect)(ref rect)).width, newItemByIndexInGroup5.Padding);
						}
						mItemList.Insert(0, newItemByIndexInGroup5);
						float x3 = loopStaggeredGridViewItem3.CachedRectTransform.anchoredPosition3D.x;
						rect = newItemByIndexInGroup5.CachedRectTransform.rect;
						float num8 = x3 + ((Rect)(ref rect)).width + newItemByIndexInGroup5.Padding;
						newItemByIndexInGroup5.CachedRectTransform.anchoredPosition3D = new Vector3(num8, newItemByIndexInGroup5.StartPosOffset, 0f);
						CheckIfNeedUpdateItemPos();
						if (num7 < mCurReadyMinItemIndex)
						{
							mCurReadyMinItemIndex = num7;
						}
						return true;
					}
					mCurReadyMinItemIndex = loopStaggeredGridViewItem3.ItemIndexInGroup;
					mNeedCheckNextMinItem = false;
				}
			}
			if (mViewPortRectLocalCorners[1].x - val7.x < distanceForNew1)
			{
				if (loopStaggeredGridViewItem4.ItemIndexInGroup > mCurReadyMaxItemIndex)
				{
					mCurReadyMaxItemIndex = loopStaggeredGridViewItem4.ItemIndexInGroup;
					mNeedCheckNextMaxItem = true;
				}
				int num9 = loopStaggeredGridViewItem4.ItemIndexInGroup + 1;
				if (num9 >= mItemIndexMap.Count)
				{
					return false;
				}
				if (num9 <= mCurReadyMaxItemIndex || mNeedCheckNextMaxItem)
				{
					LoopStaggeredGridViewItem newItemByIndexInGroup6 = GetNewItemByIndexInGroup(num9);
					if (!((Object)(object)newItemByIndexInGroup6 == (Object)null))
					{
						if (mSupportScrollBar)
						{
							rect = newItemByIndexInGroup6.CachedRectTransform.rect;
							SetItemSize(num9, ((Rect)(ref rect)).width, newItemByIndexInGroup6.Padding);
						}
						mItemList.Add(newItemByIndexInGroup6);
						float x4 = loopStaggeredGridViewItem4.CachedRectTransform.anchoredPosition3D.x;
						rect = loopStaggeredGridViewItem4.CachedRectTransform.rect;
						float num10 = x4 - ((Rect)(ref rect)).width - loopStaggeredGridViewItem4.Padding;
						newItemByIndexInGroup6.CachedRectTransform.anchoredPosition3D = new Vector3(num10, newItemByIndexInGroup6.StartPosOffset, 0f);
						CheckIfNeedUpdateItemPos();
						if (num9 > mCurReadyMaxItemIndex)
						{
							mCurReadyMaxItemIndex = num9;
						}
						return true;
					}
					mCurReadyMaxItemIndex = loopStaggeredGridViewItem4.ItemIndexInGroup;
					mNeedCheckNextMaxItem = false;
					CheckIfNeedUpdateItemPos();
				}
			}
		}
		return false;
	}

	public bool UpdateForHorizontalListPart2(float distanceForRecycle0, float distanceForRecycle1, float distanceForNew0, float distanceForNew1)
	{
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		if (mItemTotalCount == 0)
		{
			if (mItemList.Count > 0)
			{
				RecycleAllItem();
			}
			return false;
		}
		Rect rect;
		if (mArrangeType == ListItemArrangeType.LeftToRight)
		{
			if (mItemList.Count == 0)
			{
				float num = mContainerTrans.anchoredPosition3D.x;
				if (num > 0f)
				{
					num = 0f;
				}
				int index = 0;
				float itemPos = 0f - num;
				if (mSupportScrollBar && !GetPlusItemIndexAndPosAtGivenPos(0f - num, ref index, ref itemPos))
				{
					return false;
				}
				LoopStaggeredGridViewItem newItemByIndexInGroup = GetNewItemByIndexInGroup(index);
				if ((Object)(object)newItemByIndexInGroup == (Object)null)
				{
					return false;
				}
				if (mSupportScrollBar)
				{
					int itemIndex = index;
					rect = newItemByIndexInGroup.CachedRectTransform.rect;
					SetItemSize(itemIndex, ((Rect)(ref rect)).width, newItemByIndexInGroup.Padding);
				}
				mItemList.Add(newItemByIndexInGroup);
				newItemByIndexInGroup.CachedRectTransform.anchoredPosition3D = new Vector3(itemPos, newItemByIndexInGroup.StartPosOffset, 0f);
				return true;
			}
			LoopStaggeredGridViewItem loopStaggeredGridViewItem = mItemList[mItemList.Count - 1];
			loopStaggeredGridViewItem.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
			if (((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[2]).x - mViewPortRectLocalCorners[2].x < distanceForNew1)
			{
				if (loopStaggeredGridViewItem.ItemIndexInGroup > mCurReadyMaxItemIndex)
				{
					mCurReadyMaxItemIndex = loopStaggeredGridViewItem.ItemIndexInGroup;
					mNeedCheckNextMaxItem = true;
				}
				int num2 = loopStaggeredGridViewItem.ItemIndexInGroup + 1;
				if (num2 <= mCurReadyMaxItemIndex || mNeedCheckNextMaxItem)
				{
					LoopStaggeredGridViewItem newItemByIndexInGroup2 = GetNewItemByIndexInGroup(num2);
					if (!((Object)(object)newItemByIndexInGroup2 == (Object)null))
					{
						if (mSupportScrollBar)
						{
							rect = newItemByIndexInGroup2.CachedRectTransform.rect;
							SetItemSize(num2, ((Rect)(ref rect)).width, newItemByIndexInGroup2.Padding);
						}
						mItemList.Add(newItemByIndexInGroup2);
						float x = loopStaggeredGridViewItem.CachedRectTransform.anchoredPosition3D.x;
						rect = loopStaggeredGridViewItem.CachedRectTransform.rect;
						float num3 = x + ((Rect)(ref rect)).width + loopStaggeredGridViewItem.Padding;
						newItemByIndexInGroup2.CachedRectTransform.anchoredPosition3D = new Vector3(num3, newItemByIndexInGroup2.StartPosOffset, 0f);
						CheckIfNeedUpdateItemPos();
						if (num2 > mCurReadyMaxItemIndex)
						{
							mCurReadyMaxItemIndex = num2;
						}
						return true;
					}
					mCurReadyMaxItemIndex = loopStaggeredGridViewItem.ItemIndexInGroup;
					mNeedCheckNextMaxItem = false;
					CheckIfNeedUpdateItemPos();
				}
			}
		}
		else
		{
			if (mItemList.Count == 0)
			{
				float num4 = mContainerTrans.anchoredPosition3D.x;
				if (num4 < 0f)
				{
					num4 = 0f;
				}
				int index2 = 0;
				float itemPos2 = 0f - num4;
				if (mSupportScrollBar)
				{
					if (!GetPlusItemIndexAndPosAtGivenPos(num4, ref index2, ref itemPos2))
					{
						return false;
					}
					itemPos2 = 0f - itemPos2;
				}
				LoopStaggeredGridViewItem newItemByIndexInGroup3 = GetNewItemByIndexInGroup(index2);
				if ((Object)(object)newItemByIndexInGroup3 == (Object)null)
				{
					return false;
				}
				if (mSupportScrollBar)
				{
					int itemIndex2 = index2;
					rect = newItemByIndexInGroup3.CachedRectTransform.rect;
					SetItemSize(itemIndex2, ((Rect)(ref rect)).width, newItemByIndexInGroup3.Padding);
				}
				mItemList.Add(newItemByIndexInGroup3);
				newItemByIndexInGroup3.CachedRectTransform.anchoredPosition3D = new Vector3(itemPos2, newItemByIndexInGroup3.StartPosOffset, 0f);
				return true;
			}
			LoopStaggeredGridViewItem loopStaggeredGridViewItem2 = mItemList[mItemList.Count - 1];
			loopStaggeredGridViewItem2.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
			Vector3 val = ((Transform)mViewPortRectTransform).InverseTransformPoint(mItemWorldCorners[1]);
			if (mViewPortRectLocalCorners[1].x - val.x < distanceForNew1)
			{
				if (loopStaggeredGridViewItem2.ItemIndexInGroup > mCurReadyMaxItemIndex)
				{
					mCurReadyMaxItemIndex = loopStaggeredGridViewItem2.ItemIndexInGroup;
					mNeedCheckNextMaxItem = true;
				}
				int num5 = loopStaggeredGridViewItem2.ItemIndexInGroup + 1;
				if (num5 <= mCurReadyMaxItemIndex || mNeedCheckNextMaxItem)
				{
					LoopStaggeredGridViewItem newItemByIndexInGroup4 = GetNewItemByIndexInGroup(num5);
					if (!((Object)(object)newItemByIndexInGroup4 == (Object)null))
					{
						if (mSupportScrollBar)
						{
							rect = newItemByIndexInGroup4.CachedRectTransform.rect;
							SetItemSize(num5, ((Rect)(ref rect)).width, newItemByIndexInGroup4.Padding);
						}
						mItemList.Add(newItemByIndexInGroup4);
						float x2 = loopStaggeredGridViewItem2.CachedRectTransform.anchoredPosition3D.x;
						rect = loopStaggeredGridViewItem2.CachedRectTransform.rect;
						float num6 = x2 - ((Rect)(ref rect)).width - loopStaggeredGridViewItem2.Padding;
						newItemByIndexInGroup4.CachedRectTransform.anchoredPosition3D = new Vector3(num6, newItemByIndexInGroup4.StartPosOffset, 0f);
						CheckIfNeedUpdateItemPos();
						if (num5 > mCurReadyMaxItemIndex)
						{
							mCurReadyMaxItemIndex = num5;
						}
						return true;
					}
					mCurReadyMaxItemIndex = loopStaggeredGridViewItem2.ItemIndexInGroup;
					mNeedCheckNextMaxItem = false;
					CheckIfNeedUpdateItemPos();
				}
			}
		}
		return false;
	}

	public float GetContentPanelSize()
	{
		float num = ((mItemPosMgr.mTotalSize > 0f) ? (mItemPosMgr.mTotalSize - mLastItemPadding) : 0f);
		if (num < 0f)
		{
			num = 0f;
		}
		return num;
	}

	public float GetShownItemPosMaxValue()
	{
		if (mItemList.Count == 0)
		{
			return 0f;
		}
		LoopStaggeredGridViewItem loopStaggeredGridViewItem = mItemList[mItemList.Count - 1];
		if (mArrangeType == ListItemArrangeType.TopToBottom)
		{
			return Mathf.Abs(loopStaggeredGridViewItem.BottomY);
		}
		if (mArrangeType == ListItemArrangeType.BottomToTop)
		{
			return Mathf.Abs(loopStaggeredGridViewItem.TopY);
		}
		if (mArrangeType == ListItemArrangeType.LeftToRight)
		{
			return Mathf.Abs(loopStaggeredGridViewItem.RightX);
		}
		if (mArrangeType == ListItemArrangeType.RightToLeft)
		{
			return Mathf.Abs(loopStaggeredGridViewItem.LeftX);
		}
		return 0f;
	}

	public void CheckIfNeedUpdateItemPos()
	{
		if (mItemList.Count == 0)
		{
			return;
		}
		if (mArrangeType == ListItemArrangeType.TopToBottom)
		{
			LoopStaggeredGridViewItem loopStaggeredGridViewItem = mItemList[0];
			LoopStaggeredGridViewItem loopStaggeredGridViewItem2 = mItemList[mItemList.Count - 1];
			if (loopStaggeredGridViewItem.TopY > 0f || (loopStaggeredGridViewItem.ItemIndexInGroup == mCurReadyMinItemIndex && loopStaggeredGridViewItem.TopY != 0f))
			{
				UpdateAllShownItemsPos();
				return;
			}
			float contentPanelSize = GetContentPanelSize();
			if (0f - loopStaggeredGridViewItem2.BottomY > contentPanelSize || (loopStaggeredGridViewItem2.ItemIndexInGroup == mCurReadyMaxItemIndex && 0f - loopStaggeredGridViewItem2.BottomY != contentPanelSize))
			{
				UpdateAllShownItemsPos();
			}
		}
		else if (mArrangeType == ListItemArrangeType.BottomToTop)
		{
			LoopStaggeredGridViewItem loopStaggeredGridViewItem3 = mItemList[0];
			LoopStaggeredGridViewItem loopStaggeredGridViewItem4 = mItemList[mItemList.Count - 1];
			if (loopStaggeredGridViewItem3.BottomY < 0f || (loopStaggeredGridViewItem3.ItemIndexInGroup == mCurReadyMinItemIndex && loopStaggeredGridViewItem3.BottomY != 0f))
			{
				UpdateAllShownItemsPos();
				return;
			}
			float contentPanelSize2 = GetContentPanelSize();
			if (loopStaggeredGridViewItem4.TopY > contentPanelSize2 || (loopStaggeredGridViewItem4.ItemIndexInGroup == mCurReadyMaxItemIndex && loopStaggeredGridViewItem4.TopY != contentPanelSize2))
			{
				UpdateAllShownItemsPos();
			}
		}
		else if (mArrangeType == ListItemArrangeType.LeftToRight)
		{
			LoopStaggeredGridViewItem loopStaggeredGridViewItem5 = mItemList[0];
			LoopStaggeredGridViewItem loopStaggeredGridViewItem6 = mItemList[mItemList.Count - 1];
			if (loopStaggeredGridViewItem5.LeftX < 0f || (loopStaggeredGridViewItem5.ItemIndexInGroup == mCurReadyMinItemIndex && loopStaggeredGridViewItem5.LeftX != 0f))
			{
				UpdateAllShownItemsPos();
				return;
			}
			float contentPanelSize3 = GetContentPanelSize();
			if (loopStaggeredGridViewItem6.RightX > contentPanelSize3 || (loopStaggeredGridViewItem6.ItemIndexInGroup == mCurReadyMaxItemIndex && loopStaggeredGridViewItem6.RightX != contentPanelSize3))
			{
				UpdateAllShownItemsPos();
			}
		}
		else
		{
			if (mArrangeType != ListItemArrangeType.RightToLeft)
			{
				return;
			}
			LoopStaggeredGridViewItem loopStaggeredGridViewItem7 = mItemList[0];
			LoopStaggeredGridViewItem loopStaggeredGridViewItem8 = mItemList[mItemList.Count - 1];
			if (loopStaggeredGridViewItem7.RightX > 0f || (loopStaggeredGridViewItem7.ItemIndexInGroup == mCurReadyMinItemIndex && loopStaggeredGridViewItem7.RightX != 0f))
			{
				UpdateAllShownItemsPos();
				return;
			}
			float contentPanelSize4 = GetContentPanelSize();
			if (0f - loopStaggeredGridViewItem8.LeftX > contentPanelSize4 || (loopStaggeredGridViewItem8.ItemIndexInGroup == mCurReadyMaxItemIndex && 0f - loopStaggeredGridViewItem8.LeftX != contentPanelSize4))
			{
				UpdateAllShownItemsPos();
			}
		}
	}

	public void UpdateAllShownItemsPos()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		int count = mItemList.Count;
		if (count == 0)
		{
			return;
		}
		Rect rect;
		if (mArrangeType == ListItemArrangeType.TopToBottom)
		{
			float num = 0f;
			if (mSupportScrollBar)
			{
				num = 0f - GetItemPos(mItemList[0].ItemIndexInGroup);
			}
			float num2 = num;
			for (int i = 0; i < count; i++)
			{
				LoopStaggeredGridViewItem loopStaggeredGridViewItem = mItemList[i];
				loopStaggeredGridViewItem.CachedRectTransform.anchoredPosition3D = new Vector3(loopStaggeredGridViewItem.StartPosOffset, num2, 0f);
				float num3 = num2;
				rect = loopStaggeredGridViewItem.CachedRectTransform.rect;
				num2 = num3 - ((Rect)(ref rect)).height - loopStaggeredGridViewItem.Padding;
			}
		}
		else if (mArrangeType == ListItemArrangeType.BottomToTop)
		{
			float num4 = 0f;
			if (mSupportScrollBar)
			{
				num4 = GetItemPos(mItemList[0].ItemIndexInGroup);
			}
			float num5 = num4;
			for (int j = 0; j < count; j++)
			{
				LoopStaggeredGridViewItem loopStaggeredGridViewItem2 = mItemList[j];
				loopStaggeredGridViewItem2.CachedRectTransform.anchoredPosition3D = new Vector3(loopStaggeredGridViewItem2.StartPosOffset, num5, 0f);
				float num6 = num5;
				rect = loopStaggeredGridViewItem2.CachedRectTransform.rect;
				num5 = num6 + ((Rect)(ref rect)).height + loopStaggeredGridViewItem2.Padding;
			}
		}
		else if (mArrangeType == ListItemArrangeType.LeftToRight)
		{
			float num7 = 0f;
			if (mSupportScrollBar)
			{
				num7 = GetItemPos(mItemList[0].ItemIndexInGroup);
			}
			float num8 = num7;
			for (int k = 0; k < count; k++)
			{
				LoopStaggeredGridViewItem loopStaggeredGridViewItem3 = mItemList[k];
				loopStaggeredGridViewItem3.CachedRectTransform.anchoredPosition3D = new Vector3(num8, loopStaggeredGridViewItem3.StartPosOffset, 0f);
				float num9 = num8;
				rect = loopStaggeredGridViewItem3.CachedRectTransform.rect;
				num8 = num9 + ((Rect)(ref rect)).width + loopStaggeredGridViewItem3.Padding;
			}
		}
		else if (mArrangeType == ListItemArrangeType.RightToLeft)
		{
			float num10 = 0f;
			if (mSupportScrollBar)
			{
				num10 = 0f - GetItemPos(mItemList[0].ItemIndexInGroup);
			}
			float num11 = num10;
			for (int l = 0; l < count; l++)
			{
				LoopStaggeredGridViewItem loopStaggeredGridViewItem4 = mItemList[l];
				loopStaggeredGridViewItem4.CachedRectTransform.anchoredPosition3D = new Vector3(num11, loopStaggeredGridViewItem4.StartPosOffset, 0f);
				float num12 = num11;
				rect = loopStaggeredGridViewItem4.CachedRectTransform.rect;
				num11 = num12 - ((Rect)(ref rect)).width - loopStaggeredGridViewItem4.Padding;
			}
		}
	}
}
