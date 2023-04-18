using System;
using System.Collections.Generic;
using System.Linq;

namespace Alitz.Ecs;
public abstract class SparseSet {
    protected const int InvalidDenseListIndex = -1;

    protected SparseSet() {
        DenseList = new List<Entity>();
        SparseList = new List<int>();
    }

    protected List<Entity> DenseList { get; }
    protected List<int> SparseList { get; }

    public int Count =>
        DenseList.Count;

    public IEnumerable<Entity> Entities {
        get {
            foreach (Entity entity in DenseList) {
                yield return entity;
            }
        }
    }

    public bool Contains(Entity entity) =>
        SparseListRichContains(entity).contains;

    /// <summary>
    /// Tries to remove the entity from the <see cref="DenseList"/> and replace
    /// its index in the <see cref="SparseList"/> with <see cref="InvalidDenseListIndex"/>.
    /// Returns either its former index in the <see cref="DenseList"/> or
    /// <see cref="InvalidDenseListIndex"/> if no such entity was found.
    /// </summary>
    /// <param name="entity">Entity to be removed.</param>
    /// <returns>Former <see cref="DenseList"/> index of the removed entity,
    /// or <see cref="InvalidDenseListIndex"/> if no entity was removed.</returns>
    protected int TryRemoveEntity(Entity entity) {
        (bool contains, _, int entityIndex) = SparseListRichContains(entity);
        if (!contains) {
            return InvalidDenseListIndex;
        }
        SparseList[entity.Id] = InvalidDenseListIndex;
        DenseList.RemoveAt(entityIndex);
        return entityIndex;
    }
    
    /// <summary>
    /// Tries to add the entity into the <see cref="DenseList"/> and set its
    /// <see cref="DenseList"/> index within the <see cref="SparseList"/>.
    /// If the <see cref="SparseList"/> element supposed to contain the
    /// <see cref="DenseList"/> index is not <see cref="InvalidDenseListIndex"/>,
    /// does nothing, since the entity is already contained.
    /// Returns the <see cref="DenseList"/> index of the entity that was added or was
    /// already contained in the <see cref="DenseList"/>, but never <see cref="InvalidDenseListIndex"/>.
    /// </summary>
    /// <param name="entity">Entity to be added.</param>
    /// <returns>The <see cref="DenseList"/> index of the entity added or contained.</returns>
    protected int TryAddEntity(Entity entity) {
        (bool contains, bool isInRange, int entityIndex) = SparseListRichContains(entity);
        if (!contains && !isInRange) {
            ResizeSparseList(entity.Id + 1);
            entityIndex = SparseList[entity.Id];
        }
        if (entityIndex == InvalidDenseListIndex) {
            entityIndex = SparseList[entity.Id] = DenseList.Count;
            DenseList.Add(entity);
        }
        return entityIndex;
    }
    
    /// <summary>
    /// Checks if the entity is effectively contained, or specifically that
    /// the <see cref="SparseList"/> contains a corresponding <see cref="DenseList"/> index for that
    /// entity. The first return value tuple element signifies whether the
    /// entity is contained at all. If it's false, the rest of the tuple
    /// elements are also false or invalid. The second parameter clarifies
    /// whether the entity is contained/not contained, when used as
    /// an index into the <see cref="SparseList"/>, due to falling within/outside the range of
    /// the <see cref="SparseList"/>, or due to the corresponding <see cref="SparseList"/> element
    /// not containing/containing a <see cref="InvalidDenseListIndex"/> respectively.
    /// Finally, the third tuple element is the index into the <see cref="DenseList"/> if
    /// the entity is contained, or <see cref="InvalidDenseListIndex"/> if not.
    /// </summary>
    /// <param name="entity">Entity to be checked for being contained.</param>
    /// <returns>A tuple with detailed info about whether the
    /// entity is contained, described in the summary.</returns>
    protected (bool contains, bool isInRange, int entityIndex) SparseListRichContains(Entity entity) {
        bool isInRange = entity.Id < SparseList.Count;
        int entityIndex = isInRange ? SparseList[entity.Id] : InvalidDenseListIndex;
        bool contains = isInRange && entityIndex != InvalidDenseListIndex;
        return (contains, isInRange, entityIndex);
    }
    
    /// <summary>
    /// Sets the count of elements within the sparse list
    /// to the exact count specified by the corresponding parameter.
    /// If the new count is the same as the old one, does nothing.
    /// If greater, the element count is increased, and new elements
    /// are set to <see cref="InvalidDenseListIndex"/>.
    /// If less, simply removes the extra elements.
    /// </summary>
    /// <param name="count">The new element count of the sparse list</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when count is negative.</exception>
    protected void ResizeSparseList(int count) {
        if (count < 0) {
            throw new ArgumentOutOfRangeException(nameof(count));
        }
        int currentCount = SparseList.Count;
        if (count < currentCount) {
            SparseList.RemoveRange(count, currentCount - count);
        } else {
            SparseList.EnsureCapacity(count);
            SparseList.AddRange(Enumerable.Repeat(InvalidDenseListIndex, count - currentCount));
        }
    }
}
