using NumSharp;
using OpenCvSharp;
using System.Runtime.InteropServices;
using System;
using System.Linq;
using System.Collections.Generic;

namespace core.OpenCvNumSharpConverter
{
    static class CoordinatesConverter
    {   
        /*
         Converts Cartesian coordinates to homogeneous coordinates.
         Args:
         coordinates (NDArray): the Cartesian coordinates (shape: [..., 2])

        Returns:
            NDArray: the homogenous coordinates (shape: [..., 3])
        */
        public static NDArray ToHomogenousCoordinates(NDArray coordinates)
        {
            int lastDim = coordinates.shape.Length - 1;

            var indexArray = Enumerable.Range(0, lastDim).Select(i => Slice.All).ToArray();

            var slicedCoordinates = coordinates[indexArray];

            var onesShape = coordinates.shape.Take(lastDim).Append(1).ToArray();

            var onesArray = np.ones(onesShape);

            if (!slicedCoordinates.shape.SkipLast(1).SequenceEqual(onesArray.shape.SkipLast(1)))
            {
                throw new ArgumentException("Incompatible dimensions for concatenation.");
            }

            var result = np.concatenate(new NDArray[] { slicedCoordinates, onesArray }, axis: -1);

            return result;
        }
    public static NDArray FromHomogeneousCoordinates(NDArray coordinates)
    {
        var lastDim = coordinates.shape.Length - 1;

        // Extract the last column (homogeneous coordinate) for each 3D vector
        var homogeneousColumn = coordinates[$":, :, {lastDim}"];

        // Divide the first two columns by the homogeneous coordinate
        var result = coordinates[$":, :, :{lastDim}"] / homogeneousColumn[$"...", np.newaxis];

        
        Console.WriteLine(" Shape Homogenous {0} {1} {2}",result.shape[0],result.shape[1],result.shape[2]);

        for(int i = 0 ; i < result.shape[0]; i++)
        {
            for(int j = 0 ; j < result.shape[1]; j++)
            {
                Console.WriteLine("[{0} {1}]",result[i][j][0], result[i][j][1]);
            }
            Console.WriteLine("\n");
        }
        return result;
    }

}
}