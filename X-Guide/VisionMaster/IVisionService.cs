using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VM.Core;
using VMControls.Interface;

namespace X_Guide.VisionMaster
{
    public interface IVisionService
    {
        /// <summary>
        /// Sends a command to the client service to get the visual center point.
        /// Waits for a response from the client service and returns the center point.
        /// Throws an exception if no response is received within 5 seconds.
        /// </summary>
        /// <returns>The center point of the visual. Return null if visual center is not found.</returns>
        /// <exception cref="Exception">Thrown when no response is received from the client service within 5 seconds.</exception>
        Task<Point> GetVisCenter();

        /// <summary>
        /// Imports a solution file from the specified file path.
        /// </summary>
        /// <param name="filepath">The path to the solution file to import.</param>
        /// <exception cref="System.TypeInitializationException">Thrown if there is an error initializing the VmSolution class.</exception>
        Task ImportSolAsync(string filepath);

        /// <summary>
        /// Runs the procedure with the specified name and returns an instance of IVmModule.
        /// </summary>
        /// <param name="name">The name of the procedure to run.</param>
        /// <param name="continuous">Whether the procedure should run continuously.</param>
        /// <returns>An instance of IVmModule representing the procedure that was run, or null if the procedure was not found.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if the specified procedure is not found.</exception>
        Task<IVmModule> RunProcedure(string name, bool continuous = false);

        List<VmModule> GetModules(VmProcedure vmProcedure);

        VmProcedure GetProcedure(string name);

        Task<List<VmProcedure>> GetAllProcedures();

        VmModule GetCameras();
    }
}