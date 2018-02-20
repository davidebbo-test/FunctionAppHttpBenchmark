# http benchmark test function app

It has one function, `/api/HttpTest`, which  default is a no-op.

The following query sttring parameters can be added to modify the behavior:

- `/api/HttpTest?AllocInMB=10`: allocates 10 MB and hold on to it till the end of the request
- `/api/HttpTest?SleepInMS=1000`: async sleep for one second
- `/api/HttpTest?MatrixSize=700`: matrix multiplication of size 700. Might take 5 or 6 seconds.
- `/api/HttpTest?Requests=10`: make 10 async outgoing http requests to microsoft.com.

Those can be combined, e.g. `/api/HttpTest?AllocInMB=10&SleepInMS=1000` will allocated 10 MB and then sleep 1 second while holding on to the memory.
