# Will Riazati
# February 17 2014
# Expectation-Maximization Algorithm

import math

#########################
#        File I/O       #
#########################

with open("x.txt") as file:
    x = [x.rstrip('\n').replace(" ","") for x in open("x.txt")]

with open("y.txt") as file:
    y = [y.rstrip('\n') for y in open("y.txt")]

T = len(x)    # T is the number of tests/examples run ~ 267
N = len(x[0]) # N is the number of inputs ~ 23
P = [1/N] * N # P_i is the probability vector
new_P = [1/N] * N

#########################
#       Equations       #
#########################

#   y_t * xi_t * p_i
def EM_Numerator(t, p, i):
    return int(y[t]) * int(x[t][i]) * p[i]

#   1 - product j=1..N (1-p[j])^xtj
def EM_Denominator(t,p,i):
    product = 1
    for j in range(0,N):
        product *= (1-p[j])**int(x[t][j])
    return 1-(product)

#   sum of Xi from t=0..T
def EM_Ti(i):
    sum = 0
    for t in range(0,T):
        sum += int(x[t][i])
    return sum

#   EM_Ti * EM_Numerator / EM_Denominator
def EM_Update(p, i):
    sum = 0
    for t in range(0,T):
        sum += EM_Numerator(t,p,i) / EM_Denominator(t,p,i)
    return sum / EM_Ti(i)

def Loglikelihood(p):
    sum = 0
    for t in range(0,T):
        sum += math.log(NoisyOrCPT(t, p))
    return float((1/T)) * sum

#   P(Y=yt|X) = 1 - product n=1..N (i-pi)^Xi
def NoisyOrCPT(t, p):
    product = 1
    for i in range(0,N):
        product *= (1-p[i])**int(x[t][i])
    if(int(y[t]) == 1):
        return 1-product
    return 1-(1-product)

#   P(Y=1|X) = 1 - product n=1..N (i-pi)^Xi
def NoisyOrCPT2(t, p):
    product = 1
    for i in range(0,N):
        product *= (1-p[i])**int(x[t][i])
    var = 1-product
    return 1-product

def mistakeCounter(p):
    mistakes = 0
    for t in range(0,T):
        if(int(y[t]) == 0 and NoisyOrCPT2(t, p) >= .5):
            mistakes += 1
        elif(int(y[t]) == 1 and NoisyOrCPT2(t, p) <= .5):
            mistakes += 1
    return mistakes

#############################
#       Program Start       #
#############################

def main():
    global P
    for k in range(0, 513):
        for j in range(0,10):
            if(k == 2**j or k == 0):
                print("iteration: " + str(k) + "\t mistakes: " + str(mistakeCounter(P))  + "\t Loglikelihood: " + str(Loglikelihood(P)))
                break
        for i in range(0,N):
            new_P[i] = EM_Update(P, i)
        P = new_P[:]
    print
    for i in range(0, len(P)):
        print("P["+str(i)+"]: " + str(P[i]))

main()

#######################
#   PRINTED RESULTS   #
#######################
# iteration: 0	    mistakes: 195	 Loglikelihood: -1.044559748133717
# iteration: 1	    mistakes: 60	 Loglikelihood: -0.504940510120726
# iteration: 2	    mistakes: 43	 Loglikelihood: -0.4107637741779621
# iteration: 4	    mistakes: 42	 Loglikelihood: -0.36512717428723324
# iteration: 8	    mistakes: 44	 Loglikelihood: -0.34766321194257643
# iteration: 16	    mistakes: 40	 Loglikelihood: -0.33467666667097906
# iteration: 32	    mistakes: 37	 Loglikelihood: -0.3225926894510678
# iteration: 64	    mistakes: 37	 Loglikelihood: -0.3148310623857991
# iteration: 128	mistakes: 36	 Loglikelihood: -0.3111558174240999
# iteration: 256	mistakes: 36	 Loglikelihood: -0.3101611042419866
# iteration: 512	mistakes: 36	 Loglikelihood: -0.3099902571462037
# P[0]: 7.824621647237339e-05
# P[1]: 0.004777944388776733
# P[2]: 2.4521455102868607e-11
# P[3]: 0.2653570734311797
# P[4]: 1.4623161752097231e-05
# P[5]: 0.009410366758680422
# P[6]: 0.24031517645853773
# P[7]: 0.11340131304468497
# P[8]: 0.00014180057655926225
# P[9]: 0.5234865055311492
# P[10]: 0.40732379740100716
# P[11]: 8.801225021570861e-08
# P[12]: 0.6158137869509409
# P[13]: 5.843269057817708e-06
# P[14]: 0.044749056792663354
# P[15]: 0.5899904478422022
# P[16]: 0.9999999999999938
# P[17]: 0.9999999826612166
# P[18]: 4.010669453946743e-09
# P[19]: 0.46299944706241125
# P[20]: 0.3531950721584744
# P[21]: 0.5248687213124325
# P[22]: 0.19475714050451626