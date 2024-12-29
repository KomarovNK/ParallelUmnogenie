using System;
using MPI;

class PingPong
{
    static void Main(string[] args)
    {
        MPI.Environment.Run(ref args, comm =>
        {
            if (comm.Rank == 0)
            {
                Console.WriteLine("Rank 0 is alive and running on " + MPI.Environment.ProcessorName);
                for (int dest = 1; dest < comm.Size; ++dest)
                {
                    Console.Write("Pinging process with rank " + dest + "...");
                    comm.Send("Ping!", dest, 0);
                    string destHostname = comm.Receive<string>(dest, 1);
                    Console.WriteLine(" Pong!");
                    Console.WriteLine("  Rank " + dest + " is alive and running on " + destHostname);
                }
            }
            else
            {
                comm.Receive<string>(0, 0);
                comm.Send(MPI.Environment.ProcessorName, 0, 1);
            }
        });
    }
}
