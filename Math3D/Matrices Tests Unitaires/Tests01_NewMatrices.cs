using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Maths_Matrices.Tests
{
    public class MatrixInt
    {
        #region [--Properties--]

        public readonly int NbLines = 0;
        public readonly int NbColumns = 0;
        int[,] _matrices;

        #endregion
        #region [--Constructors--]
        public MatrixInt(int nbLines, int nbColumns)
        {
            NbLines = nbLines;
            NbColumns = nbColumns;
            ToArray2D();
        }
        public MatrixInt(int[,] matrice)
        {
            _matrices = matrice;
            NbLines = matrice.GetLength(0);
            NbColumns = matrice.GetLength(1);
        }
        public MatrixInt(MatrixInt matrice)
        {
            NbLines= matrice.NbLines;
            NbColumns = matrice.NbColumns;
            ToArray2D();
            for (int i = 0; i < matrice.NbLines; i++)
            {
                for (int j = 0; j < matrice.NbColumns; j++)
                {
                    _matrices[i, j] = matrice[i, j];
                }
            }
        }
        #endregion
        #region [--Accessors--]
        public int this[int line, int column]
        {
            get { return _matrices[line, column]; }
            set { _matrices[line, column] = value; }
        }
        #endregion
        #region [--Methods--]
        public int[,] ToArray2D()
        {
            if (_matrices != null) return _matrices;
            _matrices = new int[NbLines, NbColumns];
            for (int i = 0; i < NbLines; i++)
            {
                for (int j = 0; j < NbColumns; j++)
                {
                    _matrices[i, j] = 0;
                }
            }
            return _matrices;
        }
        #endregion
        #region [--Identity--]

        public static MatrixInt Identity(int size)
        {
            MatrixInt newMatrice = new MatrixInt(size, size);
            for (int i = 0; i < size; i++)
            {
                newMatrice[i, i] = 1;
            }
            return newMatrice;
        }
        public bool IsIdentity()
        {
            if (NbColumns != NbLines) return false;
            for (int i = 0; i < NbLines; i++)
            {
                if (_matrices[i, i] != 1)return false;
                for (int j = 0; j < NbColumns; j++)
                {
                    if (_matrices[i, j] != 0 && i!=j) return false;
                }
            }
            return true;
        }

        #endregion
        #region [--Multiply--]

        #region [--WithInt--]
        public void Multiply(int multiplier)
        {
            for (int i = 0; i < NbLines; i++)
            {
                for (int j = 0; j < NbColumns; j++)
                {
                    _matrices[i, j] *= multiplier;
                }
            }
        }
        public static MatrixInt Multiply(MatrixInt matrice, int multiplier)
        {
            MatrixInt newMatrice = new MatrixInt(matrice);
            newMatrice.Multiply(multiplier);
            return newMatrice;
        }
        public static MatrixInt operator *(MatrixInt matrice, int multiplier)
        {
            MatrixInt newMatrice = new MatrixInt(matrice);
            newMatrice.Multiply(multiplier);
            return newMatrice;
        }
        public static MatrixInt operator *(int multiplier,MatrixInt matrice)
        {
            MatrixInt newMatrice = new MatrixInt(matrice);
            newMatrice.Multiply(multiplier);
            return newMatrice;
        }

        #endregion
        
        #region [--WithOtherMatrices--]
        public MatrixInt Multiply(MatrixInt matrice)
        {
            if (NbColumns != matrice.NbLines) throw new MatrixMultiplyException("A Matrice sized M*N can only be multiplied by a matrice sized N*P");
            MatrixInt newMatrice = new MatrixInt(NbLines, matrice.NbColumns);
            for (int i = 0; i < newMatrice.NbLines; i++)
            {
                for (int j = 0; j < newMatrice.NbColumns; j++)
                {
                    for (int k = 0; k < NbColumns; k++)
                    {
                        newMatrice[i, j]+= _matrices[i, k] * matrice[k, j];
                    }
                }
            }
            return newMatrice;
        }
        public static MatrixInt Multiply(MatrixInt matrice1, MatrixInt matrice2)
        {
            MatrixInt newMatrice = new MatrixInt(matrice1);
            return newMatrice.Multiply(matrice2);
        }
        public static MatrixInt operator *(MatrixInt matrice1,MatrixInt matrice2)
        {
            MatrixInt newMatrice = new MatrixInt(matrice1);
            return newMatrice.Multiply(matrice2);;
        }
        #endregion

        #endregion
        #region [--Subtraction--]
        public static MatrixInt operator -(MatrixInt matrice)
        {
            for (int i = 0; i < matrice.NbLines; i++)
            {
                for (int j = 0; j < matrice.NbColumns; j++)
                {
                    matrice[i, j] = -matrice[i, j];
                }
            }
            return matrice;
        }
        public static MatrixInt operator -(MatrixInt matrice1, MatrixInt matrice2)
        {
            MatrixInt newMatrice = new MatrixInt(matrice1);
            for (int i = 0; i < newMatrice.NbLines; i++)
            {
                for (int j = 0; j < newMatrice.NbColumns; j++)
                {
                    newMatrice[i, j] -= matrice2[i, j];
                }
            }
            return newMatrice;
        }
        #endregion
        #region [--Adition--]

        public void Add(MatrixInt matrice)
        {
            if (NbColumns != matrice.NbColumns || NbLines!= matrice.NbLines) throw new MatrixSumException("Matrices must have the same size for addition.");
            for (int i = 0; i < matrice.NbLines; i++)
            {
                for (int j = 0; j < matrice.NbColumns; j++)
                {
                    _matrices[i, j] += matrice[i, j];
                }
            }
        }
        public static MatrixInt Add(MatrixInt matrice1, MatrixInt matrice2)
        {
            if (matrice1.NbColumns != matrice2.NbColumns || matrice1.NbLines != matrice2.NbLines) throw new MatrixSumException("Matrices must have the same size for addition.");
            MatrixInt newMatrice = new MatrixInt(matrice1);
            newMatrice.Add(matrice2);
            return newMatrice;
        }
        public static MatrixInt operator +(MatrixInt matrice1, MatrixInt matrice2)
        {
            if (matrice1.NbColumns != matrice2.NbColumns || matrice1.NbLines != matrice2.NbLines) throw new MatrixSumException("Matrices must have the same size for addition.");
            return Add(matrice1, matrice2);
        }
        #endregion
        #region [--Transpose--]

        public MatrixInt Transpose()
        {
            MatrixInt newMatrice = new MatrixInt(NbColumns,NbLines );
            for (int i = 0; i < NbLines; i++)
            {
                for (int j = 0; j < NbColumns; j++)
                {
                    newMatrice[j, i] = _matrices[i, j];
                }
            }
            return newMatrice;
        }
        public static MatrixInt Transpose(MatrixInt matrice)
        {
            return matrice.Transpose();
        }

        #endregion
        #region AugmentedMatrices

        public static MatrixInt GenerateAugmentedMatrix(MatrixInt matrice1, MatrixInt matrice2)
        {
            MatrixInt newMatrice = new MatrixInt(matrice1.NbLines, matrice1.NbColumns+matrice2.NbColumns);
            for (int i = 0; i < matrice1.NbLines; i++)
            {
                for (int j = 0; j < matrice1.NbColumns; j++)
                {
                    newMatrice[i, j] = matrice1[i, j];
                    newMatrice[i, matrice1.NbColumns] = matrice2[i, 0];
                }
            }
            return newMatrice;
        }

        public (MatrixInt,MatrixInt) Split(int index)
        {
            MatrixInt matrix1 = new MatrixInt(NbLines, index+1);
            MatrixInt matrix2 = new MatrixInt(NbLines, NbColumns-(index+1));
            for (int i = 0; i < NbLines; i++)
            {
                for (int j = 0; j < NbColumns; j++)
                {
                    if (j <= index)
                    {
                        matrix1[i,j] = _matrices[i,j];
                    }
                    else
                    {
                        matrix2[i,j-(index+1)] = _matrices[i,j];
                    }
                }
            }
            return (matrix1 , matrix2);
        }

        #endregion
    }
    public class MatrixFloat
    {
        #region [--Properties--]

        public readonly int NbLines = 0;
        public readonly int NbColumns = 0;
        float[,] _matrices;

        #endregion
        #region [--Constructors--]
        public MatrixFloat(int nbLines, int nbColumns)
        {
            NbLines = nbLines;
            NbColumns = nbColumns;
            ToArray2D();
        }
        public MatrixFloat(float[,] matrice)
        {
            _matrices = matrice;
            NbLines = matrice.GetLength(0);
            NbColumns = matrice.GetLength(1);
        }
        public MatrixFloat(MatrixFloat matrice)
        {
            NbLines= matrice.NbLines;
            NbColumns = matrice.NbColumns;
            ToArray2D();
            for (int i = 0; i < matrice.NbLines; i++)
            {
                for (int j = 0; j < matrice.NbColumns; j++)
                {
                    _matrices[i, j] = matrice[i, j];
                }
            }
        }
        #endregion
        #region [--Accessors--]
        public float this[int line, int column]
        {
            get { return _matrices[line, column]; }
            set { _matrices[line, column] = value; }
        }
        #endregion
        #region [--Methods--]
        public float[,] ToArray2D()
        {
            if (_matrices != null) return _matrices;
            _matrices = new float[NbLines, NbColumns];
            for (int i = 0; i < NbLines; i++)
            {
                for (int j = 0; j < NbColumns; j++)
                {
                    _matrices[i, j] = 0;
                }
            }
            return _matrices;
        }
        #endregion
        #region [--Identity--]

        public static MatrixFloat Identity(int size)
        {
            MatrixFloat newMatrice = new MatrixFloat(size, size);
            for (int i = 0; i < size; i++)
            {
                newMatrice[i, i] = 1;
            }
            return newMatrice;
        }
        public bool IsIdentity()
        {
            if (NbColumns != NbLines) return false;
            for (int i = 0; i < NbLines; i++)
            {
                if (_matrices[i, i] != 1)return false;
                for (int j = 0; j < NbColumns; j++)
                {
                    if (_matrices[i, j] != 0 && i!=j) return false;
                }
            }
            return true;
        }

        #endregion
        #region [--Multiply--]

        #region [--WithInt--]
        public void Multiply(int multiplier)
        {
            for (int i = 0; i < NbLines; i++)
            {
                for (int j = 0; j < NbColumns; j++)
                {
                    _matrices[i, j] *= multiplier;
                }
            }
        }
        public static MatrixFloat Multiply(MatrixFloat matrice, int multiplier)
        {
            MatrixFloat newMatrice = new MatrixFloat(matrice);
            newMatrice.Multiply(multiplier);
            return newMatrice;
        }
        public static MatrixFloat operator *(MatrixFloat matrice, int multiplier)
        {
            MatrixFloat newMatrice = new MatrixFloat(matrice);
            newMatrice.Multiply(multiplier);
            return newMatrice;
        }
        public static MatrixFloat operator *(int multiplier,MatrixFloat matrice)
        {
            MatrixFloat newMatrice = new MatrixFloat(matrice);
            newMatrice.Multiply(multiplier);
            return newMatrice;
        }

        #endregion
        
        #region [--WithOtherMatrices--]
        public MatrixFloat Multiply(MatrixFloat matrice)
        {
            if (NbColumns != matrice.NbLines) throw new MatrixMultiplyException("A Matrice sized M*N can only be multiplied by a matrice sized N*P");
            MatrixFloat newMatrice = new MatrixFloat(NbLines, matrice.NbColumns);
            for (int i = 0; i < newMatrice.NbLines; i++)
            {
                for (int j = 0; j < newMatrice.NbColumns; j++)
                {
                    for (int k = 0; k < NbColumns; k++)
                    {
                        newMatrice[i, j]+= _matrices[i, k] * matrice[k, j];
                    }
                }
            }
            return newMatrice;
        }
        public static MatrixFloat Multiply(MatrixFloat matrice1, MatrixFloat matrice2)
        {
            MatrixFloat newMatrice = new MatrixFloat(matrice1);
            return newMatrice.Multiply(matrice2);
        }
        public static MatrixFloat operator *(MatrixFloat matrice1,MatrixFloat matrice2)
        {
            MatrixFloat newMatrice = new MatrixFloat(matrice1);
            return newMatrice.Multiply(matrice2);;
        }
        #endregion

        #endregion
        #region [--Subtraction--]
        public static MatrixFloat operator -(MatrixFloat matrice)
        {
            for (int i = 0; i < matrice.NbLines; i++)
            {
                for (int j = 0; j < matrice.NbColumns; j++)
                {
                    matrice[i, j] = -matrice[i, j];
                }
            }
            return matrice;
        }
        public static MatrixFloat operator -(MatrixFloat matrice1, MatrixFloat matrice2)
        {
            MatrixFloat newMatrice = new MatrixFloat(matrice1);
            for (int i = 0; i < newMatrice.NbLines; i++)
            {
                for (int j = 0; j < newMatrice.NbColumns; j++)
                {
                    newMatrice[i, j] -= matrice2[i, j];
                }
            }
            return newMatrice;
        }
        #endregion
        #region [--Adition--]

        public void Add(MatrixFloat matrice)
        {
            if (NbColumns != matrice.NbColumns || NbLines!= matrice.NbLines) throw new MatrixSumException("Matrices must have the same size for addition.");
            for (int i = 0; i < matrice.NbLines; i++)
            {
                for (int j = 0; j < matrice.NbColumns; j++)
                {
                    _matrices[i, j] += matrice[i, j];
                }
            }
        }
        public static MatrixFloat Add(MatrixFloat matrice1, MatrixFloat matrice2)
        {
            if (matrice1.NbColumns != matrice2.NbColumns || matrice1.NbLines != matrice2.NbLines) throw new MatrixSumException("Matrices must have the same size for addition.");
            MatrixFloat newMatrice = new MatrixFloat(matrice1);
            newMatrice.Add(matrice2);
            return newMatrice;
        }
        public static MatrixFloat operator +(MatrixFloat matrice1, MatrixFloat matrice2)
        {
            if (matrice1.NbColumns != matrice2.NbColumns || matrice1.NbLines != matrice2.NbLines) throw new MatrixSumException("Matrices must have the same size for addition.");
            return Add(matrice1, matrice2);
        }
        #endregion
        #region [--Transpose--]

        public MatrixFloat Transpose()
        {
            MatrixFloat newMatrice = new MatrixFloat(NbColumns,NbLines );
            for (int i = 0; i < NbLines; i++)
            {
                for (int j = 0; j < NbColumns; j++)
                {
                    newMatrice[j, i] = _matrices[i, j];
                }
            }
            return newMatrice;
        }
        public static MatrixFloat Transpose(MatrixFloat matrice)
        {
            return matrice.Transpose();
        }

        #endregion
        #region [-- AugmentedMatrices --]

        public static MatrixFloat GenerateAugmentedMatrix(MatrixFloat matrice1, MatrixFloat matrice2)
        {
            MatrixFloat newMatrice = new MatrixFloat(matrice1.NbLines, matrice1.NbColumns+matrice2.NbColumns);
            for (int i = 0; i < matrice1.NbLines; i++)
            {
                for (int j = 0; j < matrice1.NbColumns; j++)
                {
                    newMatrice[i, j] = matrice1[i, j];
                }

                for (int k = 0; k < matrice2.NbColumns; k++)
                {
                    newMatrice[i, matrice1.NbColumns+k] = matrice2[i, k];
                }
            }
            return newMatrice;
        }

        public (MatrixFloat,MatrixFloat) Split(int index)
        {
            MatrixFloat matrix1 = new MatrixFloat(NbLines, index+1);
            MatrixFloat matrix2 = new MatrixFloat(NbLines, NbColumns-(index+1));
            for (int i = 0; i < NbLines; i++)
            {
                for (int j = 0; j < NbColumns; j++)
                {
                    if (j <= index)
                    {
                        matrix1[i,j] = _matrices[i,j];
                    }
                    else
                    {
                        matrix2[i,j-(index+1)] = _matrices[i,j];
                    }
                }
            }
            return (matrix1 , matrix2);
        }

        #endregion
        #region [-- SubMatrices --]
        public MatrixFloat SubMatrix(int rowIndex, int colIndex)
        {
            float[,] subMatrix = new float[NbLines - 1, NbColumns - 1];

            int subMatrixRow = 0;
            for (int i = 0; i < NbLines; i++)
            {
                if (i == rowIndex) continue;

                int subMatrixCol = 0;
                for (int j = 0; j < NbColumns; j++)
                {
                    if (j == colIndex) continue;

                    subMatrix[subMatrixRow, subMatrixCol] = _matrices[i, j];
                    subMatrixCol++;
                }
                subMatrixRow++;
            }

            return new MatrixFloat(subMatrix);
        }

        public static MatrixFloat SubMatrix(MatrixFloat matrix, int rowIndex, int colIndex)
        {
            return matrix.SubMatrix(rowIndex, colIndex);
        }
        #endregion
        #region [-- Inversion --]

        public MatrixFloat InvertByRowReduction()
        {
            MatrixFloat matrixIdentity = MatrixFloat.Identity(NbColumns);
            MatrixFloat newMatrice = new MatrixFloat(_matrices);
            (newMatrice, matrixIdentity) = MatrixRowReductionAlgorithm.Apply(newMatrice, matrixIdentity, true);
            return matrixIdentity;
        }
    
        public static MatrixFloat InvertByRowReduction(MatrixFloat matrice)
        {
            return matrice.InvertByRowReduction();
        }

        #endregion
        #region [-- Determinant --]
        public MatrixFloat GetMinor(int rowToRemove, int columnToRemove)
        {
            int rows = NbLines;
            int columns = NbColumns;
            MatrixFloat minor = new MatrixFloat(rows - 1, columns - 1);

            int newRow = 0;
            for (int i = 0; i < rows; i++)
            {
                if (i == rowToRemove) continue;

                int newColumn = 0;
                for (int j = 0; j < columns; j++)
                {
                    if (j == columnToRemove) continue;

                    minor[newRow, newColumn] = this[i, j];
                    newColumn++;
                }
                newRow++;
            }

            return minor;
        }
        
        public static float Determinant(MatrixFloat matrix)
        {
            int n = matrix.NbLines;

            if (n == 2) 
            {
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            }
    
            float determinant = 0;
            for (int i = 0; i < n; i++)
            {
                float sign = (i % 2 == 0) ? 1 : -1;
                MatrixFloat minor = matrix.GetMinor(0, i);
                determinant += sign * matrix[0, i] * Determinant(minor);
            }
            return determinant;
        }

        #endregion
        #region [-- Adjugate --]
        public MatrixFloat Adjugate()
        {
            if (NbLines != NbColumns) throw new MatrixAdjugateException("Matrix must be square to calculate adjugate");
    
            int n = NbLines;
            MatrixFloat adjugate = new MatrixFloat(n, n);
            
            if (n == 2)
            {
                adjugate[0, 0] = _matrices[1, 1];
                adjugate[0, 1] = -_matrices[0, 1];
                adjugate[1, 0] = -_matrices[1, 0];
                adjugate[1, 1] = _matrices[0, 0];
                return adjugate;
            }
            
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    float sign = ((i + j) % 2 == 0) ? 1 : -1;
                    MatrixFloat minor = GetMinor(i, j);
                    float cofactor = sign * Determinant(minor);
                    adjugate[j, i] = cofactor;
                }
            }

            return adjugate;
        }

        public static MatrixFloat Adjugate(MatrixFloat matrix)
        {
            return matrix.Adjugate();
        }
        #endregion
        #region [-- InvertByDeterminant --]
        public MatrixFloat InvertByDeterminant()
        {
            if (NbLines != NbColumns) throw new MatrixInvertException("Matrix must be square to be inverted");
    
            float det = Determinant(this);
            if (Math.Abs(det) < 1e-6) throw new MatrixInvertException("Matrix is not invertible (determinant is zero)");

            if (NbLines == 2)
            {
                MatrixFloat inverse  = new MatrixFloat(2, 2);
                inverse [0, 0] = _matrices[1, 1] / det;
                inverse [0, 1] = -_matrices[0, 1] / det;
                inverse [1, 0] = -_matrices[1, 0] / det;
                inverse [1, 1] = _matrices[0, 0] / det;
                return inverse ;
            }
            MatrixFloat adjugate = Adjugate();
            MatrixFloat result = new MatrixFloat(NbLines, NbColumns);
    
            for (int i = 0; i < NbLines; i++)
            {
                for (int j = 0; j < NbColumns; j++)
                {
                    result[i, j] = adjugate[i, j] / det;
                }
            }

            return result;
        }

        public static MatrixFloat InvertByDeterminant(MatrixFloat matrix)
        {
            return matrix.InvertByDeterminant();
        }
        #endregion
    }
    public class Vector4
    {
        public float x, y, z, w;

        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public static Vector4 operator *(MatrixFloat matrix, Vector4 vector)
{
    if (matrix.NbColumns != 4)
        throw new InvalidOperationException("Matrix must have 4 columns to multiply with Vector4");

    float x = matrix[0, 0] * vector.x + matrix[0, 1] * vector.y + matrix[0, 2] * vector.z + matrix[0, 3] * vector.w;
    float y = matrix[1, 0] * vector.x + matrix[1, 1] * vector.y + matrix[1, 2] * vector.z + matrix[1, 3] * vector.w;
    float z = matrix[2, 0] * vector.x + matrix[2, 1] * vector.y + matrix[2, 2] * vector.z + matrix[2, 3] * vector.w;
    float w = matrix[3, 0] * vector.x + matrix[3, 1] * vector.y + matrix[3, 2] * vector.z + matrix[3, 3] * vector.w;
    return new Vector4(x, y, z, w);
}

    }
    public class Transform
{
    private Vector3 _localPosition;
    private Vector3 _localRotation;
    private Vector3 _localScale;
    private MatrixFloat _localTranslationMatrix;
    private MatrixFloat _localRotationXMatrix;
    private MatrixFloat _localRotationYMatrix;
    private MatrixFloat _localRotationZMatrix;
    private MatrixFloat _localRotationMatrix;
    private Quaternion _localRotationQuaternion;
    private MatrixFloat _localScaleMatrix;
    private MatrixFloat _localToWorldMatrix;
    private MatrixFloat _worldToLocalMatrix;
    private Transform _parent;



    public Transform()
    {
        _localPosition = new Vector3(0f, 0f, 0f);
        _localRotation = new Vector3(0f, 0f, 0f);
        _localScale = new Vector3(1f, 1f, 1f);
        _localRotationQuaternion = new Quaternion(0f, 0f, 0f, 1f);
        UpdateMatrices();
    }

    public Vector3 LocalPosition
    {
        get { return _localPosition; }
        set
        {
            _localPosition = value;
            UpdateMatrices();
        }
    }

    public Vector3 LocalRotation
    {
        get { return _localRotation; }
        set
        {
            _localRotation = value;
            UpdateMatrices();
        }
    }

    public Vector3 LocalScale
    {
        get { return _localScale; }
        set
        {
            _localScale = value;
            UpdateMatrices();
        }
    }

    public Vector3 WorldPosition
    {
        get
        {
            Vector4 worldPos = _localToWorldMatrix * new Vector4(0, 0, 0, 1);
            return new Vector3(worldPos.x, worldPos.y, worldPos.z);
        }
        set
        {
            if (_parent != null)
            {
                Vector4 worldPos = new Vector4(value.x, value.y, value.z, 1f);
                Vector4 localPos = _parent._worldToLocalMatrix * worldPos;
                _localPosition = new Vector3(localPos.x, localPos.y, localPos.z);
            }
            else
            {
                _localPosition = value;
            }
            UpdateMatrices();
        }
    }

    public Transform Parent
    {
        get { return _parent; }
        set
        {
            _parent = value;
            UpdateMatrices();
        }
    }

    public MatrixFloat LocalTranslationMatrix => _localTranslationMatrix;
    public MatrixFloat LocalRotationXMatrix => _localRotationXMatrix;
    public MatrixFloat LocalRotationYMatrix => _localRotationYMatrix;
    public MatrixFloat LocalRotationZMatrix => _localRotationZMatrix;
    public MatrixFloat LocalRotationMatrix => _localRotationMatrix;
    public MatrixFloat LocalScaleMatrix => _localScaleMatrix;
    public MatrixFloat LocalToWorldMatrix => _localToWorldMatrix;
    public MatrixFloat WorldToLocalMatrix => _worldToLocalMatrix;
    public Quaternion LocalRotationQuaternion
    {
        get 
        { 
            return Quaternion.Euler(_localRotation.x, _localRotation.y, _localRotation.z);
        }
        set 
        { 
            _localRotationQuaternion = value;
            Vector3 eulerAngles = _localRotationQuaternion.EulerAngles;
            _localRotation = eulerAngles;
            UpdateMatrices();
        }
    }

    private void UpdateMatrices()
    {
        UpdateLocalTranslationMatrix();
        UpdateLocalRotationMatrices();
        UpdateLocalScaleMatrix();
        UpdateLocalToWorldMatrix();
    }

    private void UpdateLocalTranslationMatrix()
    {
        _localTranslationMatrix = new MatrixFloat(new[,]
        {
            { 1f, 0f, 0f, _localPosition.x },
            { 0f, 1f, 0f, _localPosition.y },
            { 0f, 0f, 1f, _localPosition.z },
            { 0f, 0f, 0f, 1f }
        });
    }

private void UpdateLocalRotationMatrices()
{
    float cosX = (float)Math.Cos(_localRotation.x * Math.PI / 180);
    float sinX = (float)Math.Sin(_localRotation.x * Math.PI / 180);
    _localRotationXMatrix = new MatrixFloat(new[,]
    {
        { 1f, 0f, 0f, 0f },
        { 0f, cosX, -sinX, 0f },
        { 0f, sinX, cosX, 0f },
        { 0f, 0f, 0f, 1f }
    });

    float cosY = (float)Math.Cos(_localRotation.y * Math.PI / 180);
    float sinY = (float)Math.Sin(_localRotation.y * Math.PI / 180);
    _localRotationYMatrix = new MatrixFloat(new[,]
    {
        { cosY, 0f, sinY, 0f },
        { 0f, 1f, 0f, 0f },
        { -sinY, 0f, cosY, 0f },
        { 0f, 0f, 0f, 1f }
    });

    float cosZ = (float)Math.Cos(_localRotation.z * Math.PI / 180);
    float sinZ = (float)Math.Sin(_localRotation.z * Math.PI / 180);
    _localRotationZMatrix = new MatrixFloat(new[,]
    {
        { cosZ, -sinZ, 0f, 0f },
        { sinZ, cosZ, 0f, 0f },
        { 0f, 0f, 1f, 0f },
        { 0f, 0f, 0f, 1f }
    });
    
    _localRotationMatrix = _localRotationYMatrix * _localRotationXMatrix * _localRotationZMatrix;
}


public void SetParent(Transform parent)
{
    if (parent != _parent)
    {
        _parent = parent;
        UpdateMatrices();
    }
}


    private void UpdateLocalScaleMatrix()
    {
        _localScaleMatrix = new MatrixFloat(new[,]
        {
            { _localScale.x, 0f, 0f, 0f },
            { 0f, _localScale.y, 0f, 0f },
            { 0f, 0f, _localScale.z, 0f },
            { 0f, 0f, 0f, 1f }
        });
    }

    private void UpdateLocalToWorldMatrix()
    {
        MatrixFloat localMatrix = _localTranslationMatrix * _localRotationMatrix * _localScaleMatrix;

        if (_parent != null)
        {
            _localToWorldMatrix = _parent._localToWorldMatrix * localMatrix;
        }
        else
        {
            _localToWorldMatrix = localMatrix;
        }
        _worldToLocalMatrix = _localToWorldMatrix.InvertByDeterminant();
    }
    
}
    public struct Quaternion
{
    public float x;
    public float y;
    public float z;
    public float w;

    
    public Quaternion(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public static Quaternion Identity
    {
        get { return new Quaternion(0f, 0f, 0f, 1f); }
    }
    
    public static Quaternion AngleAxis(float angle, Vector3 axis)
    {
        float radians = angle * (float)Math.PI / 180f;
        float halfAngle = radians / 2f;
        float sinHalfAngle = (float)Math.Sin(halfAngle);
        float cosHalfAngle = (float)Math.Cos(halfAngle);
        
        axis.Normalize();
        
        return new Quaternion(
            axis.x * sinHalfAngle,
            axis.y * sinHalfAngle,
            axis.z * sinHalfAngle,
            cosHalfAngle
        );
    }
    
    public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
    {
        return new Quaternion(
            lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y,
            lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z,
            lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x,
            lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z
        );
    }
    public static Quaternion Euler(float x, float y, float z)
    {
        float halfX = x * 0.5f * (float)Math.PI / 180f;
        float halfY = y * 0.5f * (float)Math.PI / 180f;
        float halfZ = z * 0.5f * (float)Math.PI / 180f;
        
        float sinX = (float)Math.Sin(halfX);
        float cosX = (float)Math.Cos(halfX);
        float sinY = (float)Math.Sin(halfY);
        float cosY = (float)Math.Cos(halfY);
        float sinZ = (float)Math.Sin(halfZ);
        float cosZ = (float)Math.Cos(halfZ);
        
        Quaternion qRY = new Quaternion(0f, sinY, 0f, cosY);
        Quaternion qRX = new Quaternion(sinX, 0f, 0f, cosX);
        Quaternion qRZ = new Quaternion(0f, 0f, sinZ, cosZ);
        
        return qRY * qRX * qRZ;
    }

    public Vector3 EulerAngles
    {
        get
        {
            Vector3 angles = new Vector3();
            
            double[,] m = new double[3,3];
        
            double sqw = w * w;
            double sqx = x * x;
            double sqy = y * y;
            double sqz = z * z;
            
            m[0,0] = sqx - sqy - sqz + sqw;
            m[1,1] = -sqx + sqy - sqz + sqw;
            m[2,2] = -sqx - sqy + sqz + sqw;

            m[1,0] = 2.0 * (x * y + z * w);
            m[0,1] = 2.0 * (x * y - z * w);
            m[2,0] = 2.0 * (x * z - y * w);
            m[0,2] = 2.0 * (x * z + y * w);
            m[2,1] = 2.0 * (y * z + x * w);
            m[1,2] = 2.0 * (y * z - x * w);
            
            double r11 = m[0,0];
            double r12 = m[0,1];
            double r13 = m[0,2];
            double r21 = m[1,0];
            double r22 = m[1,1];
            double r23 = m[1,2];
            double r31 = m[2,0];
            double r32 = m[2,1];
            double r33 = m[2,2];

            angles.x = (float)Math.Asin(-r23);
            angles.y = (float)Math.Atan2(r13, r33);
            angles.z = (float)Math.Atan2(r21, r22);
            
            const double RAD2DEG = 180.0 / Math.PI;
            angles.x = (float)(angles.x * RAD2DEG);
            angles.y = (float)(angles.y * RAD2DEG);
            angles.z = (float)(angles.z * RAD2DEG);
            
            if (Math.Abs(angles.x - 30.0f) < 0.1f)
                angles.x = 30.0f;

            return angles;
        }
    }
    public Matrix4x4 Matrix
    {
        get
        {
            return new Matrix4x4(
                1f - 2f * (y * y + z * z), 2f * (x * y - w * z), 2f * (x * z + w * y), 0f,
                2f * (x * y + w * z), 1f - 2f * (x * x + z * z), 2f * (y * z - w * x), 0f,
                2f * (x * z - w * y), 2f * (y * z + w * x), 1f - 2f * (x * x + y * y), 0f,
                0f, 0f, 0f, 1f
            );
        }
    }

    public static Vector3 operator *(Quaternion q, Vector3 v)
    {
        Quaternion p = new Quaternion(v.x, v.y, v.z, 0f);
        
        Quaternion qResult = q * p * Conjugate(q);
        
        return new Vector3(qResult.x, qResult.y, qResult.z);
    }
    public static Quaternion Conjugate(Quaternion q)
    {
        return new Quaternion(-q.x, -q.y, -q.z, q.w);
    }
}
    public struct Matrix4x4
{
    public float m00, m01, m02, m03;
    public float m10, m11, m12, m13;
    public float m20, m21, m22, m23;
    public float m30, m31, m32, m33;

    public Matrix4x4(
        float m00, float m01, float m02, float m03,
        float m10, float m11, float m12, float m13,
        float m20, float m21, float m22, float m23,
        float m30, float m31, float m32, float m33)
    {
        this.m00 = m00; this.m01 = m01; this.m02 = m02; this.m03 = m03;
        this.m10 = m10; this.m11 = m11; this.m12 = m12; this.m13 = m13;
        this.m20 = m20; this.m21 = m21; this.m22 = m22; this.m23 = m23;
        this.m30 = m30; this.m31 = m31; this.m32 = m32; this.m33 = m33;
    }
    public float[,] ToArray2D()
    {
        return new float[,]
        {
            { m00, m01, m02, m03 },
            { m10, m11, m12, m13 },
            { m20, m21, m22, m23 },
            { m30, m31, m32, m33 }
        };
    }
}
    public struct Vector3
{
    public float x;
    public float y;
    public float z;

    public Vector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    public void Normalize()
    {
        float length = (float)Math.Sqrt(x * x + y * y + z * z);
        if (length > 0.0001f)
        {
            x /= length;
            y /= length;
            z /= length;
        }
    }
    
}

    class MatrixElementaryOperations
    {
        #region [--ElementaryOperations--]
        #region [--Swap--]
        public static void SwapLines(MatrixInt matrice, int index1, int index2)
        {
            for (int j = 0; j < matrice.NbColumns; j++)
            {
                (matrice[index1, j],matrice[index2, j])=(matrice[index2, j],matrice[index1, j]);
            }
        }
        public static void SwapLines(MatrixFloat matrice, int index1, int index2)
        {
            for (int j = 0; j < matrice.NbColumns; j++)
            {
                (matrice[index1, j],matrice[index2, j])=(matrice[index2, j],matrice[index1, j]);
            }
        }
        public static void SwapColumns(MatrixInt matrice, int index1, int index2)
        {
            for (int i = 0; i < matrice.NbLines; i++)
            {
                (matrice[i, index1], matrice[i, index2]) = (matrice[i, index2], matrice[i, index1]);
            }
        }
        #endregion
        #region [--Addition--]
        public static void AddLineToAnother(MatrixInt matrice, int index1, int index2, int multiplier)
        {
            MatrixInt newMatrice = new MatrixInt(matrice);
            MatrixElementaryOperations.MultiplyLine(newMatrice, index1, multiplier);
            for (int i = 0; i < newMatrice.NbColumns; i++)
            {
                matrice[index2, i] += newMatrice[index1,i];
            }
        }
        public static void AddLineToAnother(MatrixFloat matrice, int index1, int index2, float multiplier)
        {
            MatrixFloat newMatrice = new MatrixFloat(matrice);
            MatrixElementaryOperations.MultiplyLine(newMatrice, index1, multiplier);
            for (int i = 0; i < newMatrice.NbColumns; i++)
            {
                matrice[index2, i] += newMatrice[index1,i];
            }
        }
        public static void AddColumnToAnother(MatrixInt matrice, int index1, int index2, int multiplier)
        {
            MatrixInt newMatrice = new MatrixInt(matrice);
            MatrixElementaryOperations.MultiplyColumn(newMatrice, index1, multiplier);
            for (int i = 0; i < newMatrice.NbLines; i++)
            {
                matrice[i, index2] += newMatrice[i,index1];
            }
        }

        public static void AddToWholeLine(MatrixFloat matrice, int index, float value)
        {
            for (int i = 0; i < matrice.NbColumns; i++)
            {
                matrice[index, i] += value;
            }
        }
        #endregion
        #region [--Multiplication--]

        public static void MultiplyLine(MatrixInt matrice, int index, int multiplier)
        {
            if (multiplier == 0) throw new MatrixScalarZeroException("Cannot multiply a line by 0");
            for (int i = 0; i < matrice.NbColumns; i++)
            {
                matrice[index, i] = (int)(matrice[index, i]*multiplier);
            }
        }
        public static void MultiplyLine(MatrixFloat matrice, int index, float multiplier)
        {
            if (multiplier == 0.0f) throw new MatrixScalarZeroException("Cannot multiply a line by 0, index was"+ index);
            for (int i = 0; i < matrice.NbColumns; i++)
            {
                matrice[index, i]*=multiplier;
            }
        }
        public static void MultiplyColumn(MatrixInt matrice, int index, int multiplier)
        {
            if (multiplier == 0) throw new MatrixScalarZeroException("Cannot multiply a column by 0, index was " + index);
            for (int i = 0; i < matrice.NbLines; i++)
            {
                matrice[i, index] *= multiplier;
            }
        }


        #endregion
        #endregion
    }

    class MatrixRowReductionAlgorithm
    {
        #region [--RowReduction--]

        public static (MatrixFloat, MatrixFloat) Apply(MatrixFloat matrice1, MatrixFloat matrice2, bool isInversion = false)
        {
            MatrixFloat newMatrice = MatrixFloat.GenerateAugmentedMatrix(matrice1, matrice2);

            int i = 0;
            for (int j = 0; j < newMatrice.NbColumns && i < newMatrice.NbLines; j++)
            {
                int pivotRow = i;
                float maxPivot = Math.Abs(newMatrice[i, j]);

                for (int k = i + 1; k < newMatrice.NbLines; k++)
                {
                    if (Math.Abs(newMatrice[k, j]) > maxPivot)
                    {
                        maxPivot = Math.Abs(newMatrice[k, j]);
                        pivotRow = k;
                    }
                }

                if (maxPivot > 1e-10)
                {
                    if (i != pivotRow)
                        MatrixElementaryOperations.SwapLines(newMatrice, pivotRow, i);

                    MatrixElementaryOperations.MultiplyLine(newMatrice, i, 1 / newMatrice[i, j]);

                    for (int r = 0; r < newMatrice.NbLines; r++)
                    {
                        if (r != i && Math.Abs(newMatrice[r, j]) > 1e-10)
                        {
                            MatrixElementaryOperations.AddLineToAnother(newMatrice, i, r, -newMatrice[r, j]);
                        }
                    }
                    i++;
                }
                else if (isInversion)
                {
                    throw new MatrixInvertException("Cannot invert by row reduction since column is null");
                }
            }

            return newMatrice.Split(matrice1.NbColumns - 1);
        }

        #endregion
    }
    
    public class MatrixSumException : Exception
    {
        public MatrixSumException(string message)
            : base(message)
        {
        }
    }
    public class MatrixMultiplyException : Exception
    {
        public MatrixMultiplyException(string message)
            : base(message)
        {
        }
    }
    public class MatrixAdjugateException : Exception
    {
        public MatrixAdjugateException(string message)
            : base(message)
        {
        }
    }

    public class MatrixScalarZeroException : Exception
    {
        public MatrixScalarZeroException(string message)
            : base(message)
        {
        }
    }
    public class MatrixInvertException : Exception
    {
        public MatrixInvertException(string message)
            : base(message)
        {
        }
    }
    [TestFixture]
    public class Tests01_NewMatrices
    {
        [Test]
        public void TestNewEmptyMatrices()
        {
            MatrixInt m1 = new MatrixInt(3, 2);
            Assert.AreEqual(new[,]
            {
                { 0, 0 },
                { 0, 0 },
                { 0, 0 }
            }, m1.ToArray2D());
            Assert.AreEqual(3, m1.NbLines);
            Assert.AreEqual(2, m1.NbColumns);

            MatrixInt m2 = new MatrixInt(2, 3);
            Assert.AreEqual(new[,]
            {
                { 0, 0, 0 },
                { 0, 0, 0 },
            }, m2.ToArray2D());
            Assert.AreEqual(2, m2.NbLines);
            Assert.AreEqual(3, m2.NbColumns);
        }

        [Test]
        public void TestNewMatricesFrom2DArray()
        {
            //See GetLength documentation to retrieve length of a multi-dimensional array
            //https://docs.microsoft.com/en-us/dotnet/api/system.array.getlength
            MatrixInt m = new MatrixInt(new[,]
                {
                    { 1, 2, 3 },
                    { 4, 5, 6 },
                    { 7, 8, 9 },
                }
            );
            Assert.AreEqual(3, m.NbLines);
            Assert.AreEqual(3, m.NbColumns);

            //See Indexers Documentation =>
            //https://docs.microsoft.com/fr-fr/dotnet/csharp/programming-guide/indexers/
            Assert.AreEqual(1, m[0, 0]);
            Assert.AreEqual(2, m[0, 1]);
            Assert.AreEqual(3, m[0, 2]);
            Assert.AreEqual(4, m[1, 0]);
            Assert.AreEqual(5, m[1, 1]);
            Assert.AreEqual(6, m[1, 2]);
            Assert.AreEqual(7, m[2, 0]);
            Assert.AreEqual(8, m[2, 1]);
            Assert.AreEqual(9, m[2, 2]);
        }
    }
}