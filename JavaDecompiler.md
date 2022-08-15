```java
// class version 52.0 (52)
// access flags 0x21
public class ScalaTrial {

  // compiled from: ScalaTrial.scala

  @Lscala/reflect/ScalaSignature;(bytes="\u0006\u0005\r2A!\u0002\u0004\u0001\u0013!)\u0001\u0003\u0001C\u0001#!)A\u0003\u0001C\u0001+!)Q\u0004\u0001C\u0001=!)\u0011\u0005\u0001C\u0001E\u0009Q1kY1mCR\u0013\u0018.\u00197\u000b\u0003\u001d\u0009q\u0001P3naRLhh\u0001\u0001\u0014\u0005\u0001Q\u0001CA\u0006\u000f\u001b\u0005a!\"A\u0007\u0002\u000bM\u001c\u0017\r\\1\n\u0005=a!AB!osJ+g-\u0001\u0004=S:LGO\u0010\u000b\u0002%A\u00111\u0003A\u0007\u0002\r\u0005)Q.\u001e7uSR\u0019a#G\u000e\u0011\u0005-9\u0012B\u0001\r\r\u0005\rIe\u000e\u001e\u0005\u00065\u0009\u0001\rAF\u0001\u0002q\")AD\u0001a\u0001-\u0005\u0009\u00110\u0001\u0003qYV\u001cHc\u0001\u000c A!)!d\u0001a\u0001-!)Ad\u0001a\u0001-\u0005!\u0001/\u001b9f)\u00051\u0002")

  ATTRIBUTE ScalaSig : unknown

  ATTRIBUTE ScalaInlineInfo : unknown
  // access flags 0x19
  public final static INNERCLASS java/lang/invoke/MethodHandles$Lookup java/lang/invoke/MethodHandles Lookup
  // access flags 0x9
  public static INNERCLASS scala/util/package$chaining$ scala/util/package chaining$

  // access flags 0x1
  public multi(II)I
    // parameter final  x
    // parameter final  y
   L0
    LINENUMBER 4 L0
    ILOAD 1
    ILOAD 2
    IMUL
    IRETURN
   L1
    LOCALVARIABLE this LScalaTrial; L0 L1 0
    LOCALVARIABLE x I L0 L1 1
    LOCALVARIABLE y I L0 L1 2
    MAXSTACK = 2
    MAXLOCALS = 3

  // access flags 0x1
  public plus(II)I
    // parameter final  x
    // parameter final  y
   L0
    LINENUMBER 5 L0
    ILOAD 1
    ILOAD 2
    IADD
    IRETURN
   L1
    LOCALVARIABLE this LScalaTrial; L0 L1 0
    LOCALVARIABLE x I L0 L1 1
    LOCALVARIABLE y I L0 L1 2
    MAXSTACK = 2
    MAXLOCALS = 3

  // access flags 0x1
  public pipe()I
   L0
    LINENUMBER 8 L0
    GETSTATIC scala/util/ChainingOps$.MODULE$ : Lscala/util/ChainingOps$;
    GETSTATIC scala/util/package$chaining$.MODULE$ : Lscala/util/package$chaining$;
    GETSTATIC scala/util/ChainingOps$.MODULE$ : Lscala/util/ChainingOps$;
    GETSTATIC scala/util/package$chaining$.MODULE$ : Lscala/util/package$chaining$;
    ICONST_2
    INVOKESTATIC scala/runtime/BoxesRunTime.boxToInteger (I)Ljava/lang/Integer;
    INVOKEVIRTUAL scala/util/package$chaining$.scalaUtilChainingOps (Ljava/lang/Object;)Ljava/lang/Object;
    ALOAD 0
    INVOKEDYNAMIC apply$mcII$sp(LScalaTrial;)Lscala/runtime/java8/JFunction1$mcII$sp; [
      // handle kind 0x6 : INVOKESTATIC
      java/lang/invoke/LambdaMetafactory.altMetafactory(Ljava/lang/invoke/MethodHandles$Lookup;Ljava/lang/String;Ljava/lang/invoke/MethodType;[Ljava/lang/Object;)Ljava/lang/invoke/CallSite;
      // arguments:
      (I)I, 
      // handle kind 0x6 : INVOKESTATIC
      ScalaTrial.$anonfun$pipe$1(LScalaTrial;I)I, 
      (I)I, 
      1
    ]
    INVOKEVIRTUAL scala/util/ChainingOps$.pipe$extension (Ljava/lang/Object;Lscala/Function1;)Ljava/lang/Object;
    INVOKEVIRTUAL scala/util/package$chaining$.scalaUtilChainingOps (Ljava/lang/Object;)Ljava/lang/Object;
    ALOAD 0
    INVOKEDYNAMIC apply$mcII$sp(LScalaTrial;)Lscala/runtime/java8/JFunction1$mcII$sp; [
      // handle kind 0x6 : INVOKESTATIC
      java/lang/invoke/LambdaMetafactory.altMetafactory(Ljava/lang/invoke/MethodHandles$Lookup;Ljava/lang/String;Ljava/lang/invoke/MethodType;[Ljava/lang/Object;)Ljava/lang/invoke/CallSite;
      // arguments:
      (I)I, 
      // handle kind 0x6 : INVOKESTATIC
      ScalaTrial.$anonfun$pipe$2(LScalaTrial;I)I, 
      (I)I, 
      1
    ]
    INVOKEVIRTUAL scala/util/ChainingOps$.pipe$extension (Ljava/lang/Object;Lscala/Function1;)Ljava/lang/Object;
    INVOKESTATIC scala/runtime/BoxesRunTime.unboxToInt (Ljava/lang/Object;)I
    ISTORE 1
   L1
    LINENUMBER 9 L1
    ILOAD 1
   L2
    IRETURN
   L3
    LOCALVARIABLE x I L1 L2 1
    LOCALVARIABLE this LScalaTrial; L0 L3 0
    MAXSTACK = 5
    MAXLOCALS = 2

  // access flags 0x1019
  public final static synthetic $anonfun$pipe$1(LScalaTrial;I)I
    // parameter final synthetic  $this
    // parameter final  x$1
   L0
    LINENUMBER 8 L0
    ALOAD 0
    ILOAD 1
    ICONST_3
    INVOKEVIRTUAL ScalaTrial.plus (II)I
    IRETURN
   L1
    LOCALVARIABLE $this LScalaTrial; L0 L1 0
    LOCALVARIABLE x$1 I L0 L1 1
    MAXSTACK = 3
    MAXLOCALS = 2

  // access flags 0x1019
  public final static synthetic $anonfun$pipe$2(LScalaTrial;I)I
    // parameter final synthetic  $this
    // parameter final  x$2
   L0
    LINENUMBER 8 L0
    ALOAD 0
    ILOAD 1
    ICONST_2
    INVOKEVIRTUAL ScalaTrial.multi (II)I
    IRETURN
   L1
    LOCALVARIABLE $this LScalaTrial; L0 L1 0
    LOCALVARIABLE x$2 I L0 L1 1
    MAXSTACK = 3
    MAXLOCALS = 2

  // access flags 0x1
  public <init>()V
   L0
    LINENUMBER 3 L0
    ALOAD 0
    INVOKESPECIAL java/lang/Object.<init> ()V
    RETURN
   L1
    LOCALVARIABLE this LScalaTrial; L0 L1 0
    MAXSTACK = 1
    MAXLOCALS = 1

  // access flags 0x100A
  private static synthetic $deserializeLambda$(Ljava/lang/invoke/SerializedLambda;)Ljava/lang/Object;
    ALOAD 0
    INVOKEDYNAMIC lambdaDeserialize(Ljava/lang/invoke/SerializedLambda;)Ljava/lang/Object; [
      // handle kind 0x6 : INVOKESTATIC
      scala/runtime/LambdaDeserialize.bootstrap(Ljava/lang/invoke/MethodHandles$Lookup;Ljava/lang/String;Ljava/lang/invoke/MethodType;[Ljava/lang/invoke/MethodHandle;)Ljava/lang/invoke/CallSite;
      // arguments:
      // handle kind 0x6 : INVOKESTATIC
      ScalaTrial.$anonfun$pipe$1(LScalaTrial;I)I, 
      // handle kind 0x6 : INVOKESTATIC
      ScalaTrial.$anonfun$pipe$2(LScalaTrial;I)I
    ]
    ARETURN
    MAXSTACK = 1
    MAXLOCALS = 1
}
```
