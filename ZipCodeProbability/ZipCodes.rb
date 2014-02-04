# Creating variables
@zipCodeHash = Hash.new
@populationSum = 308576694

class EvidenceRecord
  def initialize(initalStateArray,allIllegalNumberArray)
    @in=initalStateArray
    @offSet=allIllegalNumberArray
    @openPositions = Array.new

    for i in 0..4
      if(@in[i] == nil)
        @openPositions.push(i)
      end
    end
  end

  def getIn
    @in
  end

  def getOut
    @out
  end

  def getOffSet
    @offSet
  end

  def getOpenPositions
    @openPositions
  end

  def getOnSet()
    a = Array.new
    for i in 0..9
      if(!@offSet.include?(i))
        a.push(i)
      end
    end
    return a
  end

  def printIn
    for i in 0..4
      print (@in[i] == nil) ? '_ ' : @in[i].to_s + ' '
    end
    puts
  end
end

# Adding text file into hash structure
def populateHash()
  File.open("codes.txt").each do |line|
    pair = line.to_s.split(' ')
    @zipCodeHash.merge!( pair[0].to_i => pair[1].to_i )
  end
end


def partA
  temp = @zipCodeHash.values.sort.reverse
  invert = @zipCodeHash.invert

  for i in 0..4 do
    puts invert[temp[i]]
  end

end

# P(Z=z)
def p1(z)
  return (@zipCodeHash[z].to_f)/@populationSum
end

# P(E|Z=z)
def p2(evidence,z)
  z=z.to_s
  for i in 0..4
    if(evidence.getIn[i] == nil)
      next
    end
    if( evidence.getIn[i].to_i != z[i].to_i)
      return 0
    end
  end

  if((evidence.getOffSet.size.to_i == 0.to_i))
    return 1
  end

  evidence.getOffSet.each do |x|
    evidence.getOpenPositions.each do |y|
      if(z.to_s[y.to_i].to_i == x.to_i)
        return 0
      end
    end
  end
  return 1
end

# Sumof P(E|Z=z)*P(Z=z)
def p3(evidence)
  sum=0
  @zipCodeHash.keys.each do |zip|
    sum += ( p2(evidence,zip) * p1(zip) )
  end
  return sum
end

# P(Z=z|E)
def p4(evidence,z, p3)
  numerator = p1(z) *  p2(evidence,z)
  denominator = p3
  return (numerator/denominator)
end

# P(D=di|Z=z)
def p5(d, i, z)
  if(z.to_s[i.to_i].to_i == d.to_i)
    return 1.to_f
  elsif(d.to_i==0.to_i and i.to_i==0.to_i and z.to_s.size==4.to_i)
    return 1.to_f
  else
    return 0.to_i
  end
end

# SumOf P(Di=d|Z=z)*P(Z=z|E)
def p6(d,i,evidence)
  p3=p3(evidence)

  sum=0
  @zipCodeHash.keys.each do |zip|
    sum += ( p5(d,i,zip) * p4(evidence,zip,p3) )
    #puts zip.to_s +  '--' + p5(d,i,zip).to_s + '--'+ p4(evidence,zip,p3).to_s
  end
  return sum
end

# P(D)
def p7(d,evidence)
  a=Array.new
  probabilitySum=0

  for i in 0..4
    a[i] = p6(d,i,evidence)
  end

  a.length.times do |i|
    probabilitySum += a[i]
  end

  return probabilitySum
end

# Gives P(Di) for i in 0..9
def selector(evidence)
  a=Array.new
  max=0
  maxIndex=0
  for i in 0..9
    if(evidence.getOffSet.include?(i))
      next
    end
    a[i] = p7(i,evidence)
    #puts 'P(' + i.to_s + ') = '+ (a[i]).to_s
  end
  max(a,0)
  return a
end


#Give adjusted P(Di) for i in 0..9  where D0+D1+...+D9 = 1
def probability(a)
  total=0

  for j in 0..9
    if(a[j]==nil)
      next
    end
    total += a[j]
  end
  for i in 0..9
    if(a[i]==nil)
      next
    end
    puts 'P(' + i.to_s + ') = '+ (a[i]/total).to_s
  end
  max(a,total)
end

def max(a, total)
  max=0
  maxI=0
  for i in 0..9
    if(a[i] == nil)
      next
    end
    if(a[i] > max)
      max = a[i]
      maxI = i
    end
  end

  if(total == 0)
  puts 'Initial Maximum : P(' + maxI.to_s + ') = '+ (a[maxI]).to_s
  else
  puts 'Adjusted Maximum: P(' + maxI.to_s + ') = '+ (a[maxI]/total).to_s
  end
end


populateHash()

######## PART A ########### (output commented below)
partA()
#PRINTS:
#60629 -> Chicago
#79936 -> El Paso
#11368 -> New York
#90650 -> LA
#90011 -> LA


######## PART B(i) ########### (individual output commented below)

case 9 #select i in 1..5
  when 1
    b1 = EvidenceRecord.new([nil,nil,nil,nil,nil],[])
    a = selector(b1)
    probability(a)
    #Initial Maximum : P(0) = 0.7927784436798413
    #P(0) = 0.1562043768248232
    #P(1) = 0.1182243198596102
    #P(2) = 0.1077832531689776
    #P(3) = 0.1083964534560978
    #P(4) = 0.09662059061527695
    #P(5) = 0.08772961084698444
    #P(6) = 0.08138879289621107
    #P(7) = 0.09135111464021654
    #P(8) = 0.07565711699572708
    #P(9) = 0.07664437069607509
    #Adjusted Maximum: P(0) = 0.1562043768248232

  when 2
    b2 = EvidenceRecord.new([nil,nil,nil,nil,nil],[0,3])
    a = selector(b2)
    probability(a)
    #Initial Maximum : P(1) = 0.8254199125200661
    #P(1) = 0.16508398250401365
    #P(2) = 0.15303278945933993
    #P(4) = 0.13794938954839264
    #P(5) = 0.12075967269609991
    #P(6) = 0.10029865216003697
    #P(7) = 0.11834707380637893
    #P(8) = 0.09747290755313526
    #P(9) = 0.10705553227260274
    #Adjusted Maximum: P(1) = 0.16508398250401365

  when 3
    b3 = EvidenceRecord.new([9,nil,nil,nil,5],[5,9])
    a = selector(b3)
    probability(a)
    #Initial Maximum : P(0) = 0.724376412865954
    #P(0) = 0.24145880428865132
    #P(1) = 0.12331676467429002
    #P(2) = 0.15203675882053322
    #P(3) = 0.1350489406269675
    #P(4) = 0.09908787789291658
    #P(6) = 0.07317094050019048
    #P(7) = 0.08521649218517356
    #P(8) = 0.09066342101127722
    #Adjusted Maximum: P(0) = 0.24145880428865132

  when 4
    b4 = EvidenceRecord.new([7,7,nil,nil,9],[2,3,7,9])
    a = selector(b4)
    probability(a)
    #Initial Maximum : P(4) = 0.8236691856782998
    #P(0) = 0.2450298329071489
    #P(1) = 0.04499221439693717
    #P(4) = 0.4118345928391499
    #P(5) = 0.10274257986790664
    #P(6) = 0.08680709323200143
    #P(8) = 0.10859368675685584
    #Adjusted Maximum: P(4) = 0.4118345928391499

  when 5
    b5 = EvidenceRecord.new([nil,nil,2,0,8],[0,2,3,4,5,6,7,8])
    a = selector(b5)
    probability(a)
    #Initial Maximum : P(1) = 1.2831395748778978
    #P(1) = 0.6415697874389489
    #P(9) = 0.3584302125610511
    #Adjusted Maximum: P(1) = 0.6415697874389489



  ## ADDITIONAL TEST CASES:
  when 6
    b6 = EvidenceRecord.new([1,nil,nil,1,1], [1,2,9])
    a = selector(b6)
    probability(a)
    #Initial Maximum : P(0) = 0.8286425755514082
    #P(0) = 0.4143212877757042
    #P(3) = 0.02642080749517299
    #P(4) = 0.2308627301232164
    #P(5) = 0.04607035867758815
    #P(6) = 0.09729848924074802
    #P(7) = 0.09954123259453675
    #P(8) = 0.0854850940930335
    #Adjusted Maximum: P(0) = 0.4143212877757042

  when 7
    b7 = EvidenceRecord.new([9,2,nil,2,2], [2,3,4,6,9])
    puts 'Expected: P(1) = 1.0'
    a = selector(b7)
    probability(a)
    #Expected: P(1) = 1.0
    #Initial Maximum : P(1) = 1.0
    #P(0) = 0.0
    #P(1) = 1.0
    #P(5) = 0.0
    #P(7) = 0.0
    #P(8) = 0.0
    #Adjusted Maximum: P(1) = 1.0
end


















