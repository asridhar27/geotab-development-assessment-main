package com.jokecompany;

public class ConsolePrinter {
    public static Object PrintValue;

    public ConsolePrinter Value(String value)
    {
        PrintValue = value;
        return this;
    }

    @Override
    public String toString() {
        System.out.println(PrintValue);
        return null;
    }

}
