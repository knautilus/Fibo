# Fibo
RabbitMQ + WebAPI test application.  

Two applications send messages to each other calculating Fibonacci numbers.  
First app sends Ni to the second one.  
Second app calculates Ni+1 = Ni-1 + Ni and sends it to the first one.  
First app calculates Ni + Ni+1 and sends the result to the second one.  
And so on.  

Messages from 1 to 2 go through WebAPI.  
Messages from 2 to 1 go through RabbitMQ.
