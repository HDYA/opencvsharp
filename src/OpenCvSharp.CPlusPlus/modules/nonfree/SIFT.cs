﻿/*
 * (C) 2008-2014 shimat
 * This code is licenced under the LGPL.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCvSharp.CPlusPlus
{
#if LANG_JP
    /// <summary>
    /// SIFT 実装.
    /// </summary>
#else
    /// <summary>
    /// SIFT implementation.
    /// </summary>
#endif
    public class SIFT : Feature2D
    {
        private bool disposed;

        #region Init & Disposal

        /// <summary>
        /// The SIFT constructor.
        /// </summary>
        /// <param name="nFeatures">The number of best features to retain. 
        /// The features are ranked by their scores (measured in SIFT algorithm as the local contrast)</param>
        /// <param name="nOctaveLayers">The number of layers in each octave. 3 is the value used in D. Lowe paper. 
        /// The number of octaves is computed automatically from the image resolution.</param>
        /// <param name="contrastThreshold">The contrast threshold used to filter out weak features in semi-uniform 
        /// (low-contrast) regions. The larger the threshold, the less features are produced by the detector.</param>
        /// <param name="edgeThreshold">The threshold used to filter out edge-like features. Note that the its meaning is 
        /// different from the contrastThreshold, i.e. the larger the edgeThreshold, the less features are filtered out (more features are retained).</param>
        /// <param name="sigma">The sigma of the Gaussian applied to the input image at the octave #0. 
        /// If your image is captured with a weak camera with soft lenses, you might want to reduce the number.</param>
        public SIFT(int nFeatures = 0, int nOctaveLayers = 3,
            double contrastThreshold = 0.04, double edgeThreshold = 10,
            double sigma = 1.6)
            : base()
        {
            ptr = CppInvoke.nonfree_SIFT_new(nFeatures, nOctaveLayers, 
                contrastThreshold, edgeThreshold, sigma);
        }

#if LANG_JP
        /// <summary>
        /// リソースの解放
        /// </summary>
        /// <param name="disposing">
        /// trueの場合は、このメソッドがユーザコードから直接が呼ばれたことを示す。マネージ・アンマネージ双方のリソースが解放される。
        /// falseの場合は、このメソッドはランタイムからファイナライザによって呼ばれ、もうほかのオブジェクトから参照されていないことを示す。アンマネージリソースのみ解放される。
        ///</param>
#else
        /// <summary>
        /// Releases the resources
        /// </summary>
        /// <param name="disposing">
        /// If disposing equals true, the method has been called directly or indirectly by a user's code. Managed and unmanaged resources can be disposed.
        /// If false, the method has been called by the runtime from inside the finalizer and you should not reference other objects. Only unmanaged resources can be disposed.
        /// </param>
#endif
        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                try
                {
                    // releases managed resources
                    if (disposing)
                    {
                    }
                    // releases unmanaged resources
                    if (ptr != IntPtr.Zero)
                        CppInvoke.nonfree_SIFT_delete(ptr);
                    ptr = IntPtr.Zero;
                    disposed = true;
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// returns the descriptor size in float's (64 or 128)
        /// </summary>
        /// <returns></returns>
        public int DescriptorSize
        {
            get
            {
                ThrowIfDisposed();
                return CppInvoke.nonfree_SIFT_descriptorSize(ptr);
            }
        }

        /// <summary>
        /// returns the descriptor type
        /// </summary>
        /// <returns></returns>
        public int DescriptorType
        {
            get
            {
                ThrowIfDisposed();
                return CppInvoke.nonfree_SIFT_descriptorType(ptr);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AlgorithmInfo Info
        {
            get
            {
                ThrowIfDisposed();
                IntPtr pInfo = CppInvoke.nonfree_SIFT_info(ptr);
                return new AlgorithmInfo(pInfo);
            }
        }

        #endregion

        #region Methods

#if LANG_JP
        /// <summary>
        /// 高速なマルチスケール Hesian 検出器を用いて keypoint を検出します．
        /// </summary>
        /// <param name="img"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
#else
        /// <summary>
        /// detects keypoints using fast multi-scale Hessian detector
        /// </summary>
        /// <param name="img"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
#endif
        public KeyPoint[] Run(Mat img, Mat mask)
        {
            ThrowIfDisposed();
            if (img == null)
                throw new ArgumentNullException("img");
            img.ThrowIfDisposed();

            using (StdVectorKeyPoint keypointsVec = new StdVectorKeyPoint())
            {
                CppInvoke.nonfree_SIFT_run(ptr, img.CvPtr, keypointsVec.CvPtr, Cv2.ToPtr(mask));
                return keypointsVec.ToArray();
            }
        }

#if LANG_JP
        /// <summary>
        /// keypoint を検出し，その SURF ディスクリプタを計算します．[useProvidedKeypoints = true]
        /// </summary>
        /// <param name="img"></param>
        /// <param name="mask"></param>
        /// <param name="keypoints"></param>
        /// <param name="descriptors"></param>
        /// <param name="useProvidedKeypoints"></param>
#else
        /// <summary>
        /// detects keypoints and computes the SURF descriptors for them. [useProvidedKeypoints = true]
        /// </summary>
        /// <param name="img"></param>
        /// <param name="mask"></param>
        /// <param name="keypoints"></param>
        /// <param name="descriptors"></param>
        /// <param name="useProvidedKeypoints"></param>
#endif
        public void Run(Mat img, Mat mask, out KeyPoint[] keypoints, out float[] descriptors,
            bool useProvidedKeypoints = false)
        {
            ThrowIfDisposed();
            if (img == null)
                throw new ArgumentNullException("img");
            img.ThrowIfDisposed();

            using (StdVectorKeyPoint keypointsVec = new StdVectorKeyPoint())
            using (StdVectorFloat descriptorsVec = new StdVectorFloat())
            {
                CppInvoke.nonfree_SIFT_run(ptr, img.CvPtr, Cv2.ToPtr(mask), keypointsVec.CvPtr,
                    descriptorsVec.CvPtr, useProvidedKeypoints ? 1 : 0);

                keypoints = keypointsVec.ToArray();
                descriptors = descriptorsVec.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseMat"></param>
        /// <param name="nOctaves"></param>
        /// <returns></returns>
        public Mat[] BuildGaussianPyramid(Mat baseMat, int nOctaves)
        {
            ThrowIfDisposed();
            if (baseMat == null)
                throw new ArgumentNullException("baseMat");
            baseMat.ThrowIfDisposed();

            using (StdVectorMat pyrVec = new StdVectorMat())
            {
                CppInvoke.nonfree_SIFT_buildGaussianPyramid(ptr, baseMat.CvPtr, pyrVec.CvPtr, nOctaves);
                return pyrVec.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pyr"></param>
        /// <returns></returns>
        public Mat[] BuildDoGPyramid(IEnumerable<Mat> pyr)
        {
            ThrowIfDisposed();
            if (pyr == null)
                throw new ArgumentNullException("pyr");

            IntPtr[] pyrPtrs = EnumerableEx.SelectToArray(pyr, delegate(Mat m)
            {
                if (m == null)
                    throw new ArgumentException("One of pyr element is null");
                m.ThrowIfDisposed();
                return m.CvPtr;
            });
            using (StdVectorMat dogPyrVec = new StdVectorMat())
            {
                CppInvoke.nonfree_SIFT_buildDoGPyramid(ptr, pyrPtrs, pyrPtrs.Length, dogPyrVec.CvPtr);
                return dogPyrVec.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gaussPyr"></param>
        /// <param name="dogPyr"></param>
        /// <returns></returns>
        public KeyPoint[] FindScaleSpaceExtrema(IEnumerable<Mat> gaussPyr, IEnumerable<Mat> dogPyr)
        {
            ThrowIfDisposed();
            if (gaussPyr == null)
                throw new ArgumentNullException("gaussPyr");
            if (dogPyr == null)
                throw new ArgumentNullException("dogPyr");

            IntPtr[] gaussPyrPtrs = EnumerableEx.SelectToArray(gaussPyr, delegate(Mat m)
            {
                if (m == null)
                    throw new ArgumentException("One of gaussPyr element is null");
                m.ThrowIfDisposed();
                return m.CvPtr;
            });
            IntPtr[] dogPyrPtrs = EnumerableEx.SelectToArray(dogPyr, delegate(Mat m)
            {
                if (m == null)
                    throw new ArgumentException("One of dogPyr element is null");
                m.ThrowIfDisposed();
                return m.CvPtr;
            });

            using (StdVectorKeyPoint keyPointsVec = new StdVectorKeyPoint())
            {
                CppInvoke.nonfree_SIFT_findScaleSpaceExtrema(ptr, gaussPyrPtrs, gaussPyrPtrs.Length,
                    dogPyrPtrs, dogPyrPtrs.Length, keyPointsVec.CvPtr);
                return keyPointsVec.ToArray();
            }
        }

        #endregion
    }
}
