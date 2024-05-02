package com.jokecompany;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.URISyntaxException;
import java.util.Hashtable;

public class Main {

    static String[] results = new String[50];
    static char key;
    static Hashtable<String, String> names = new Hashtable<>();
    static ConsolePrinter printer = new ConsolePrinter();

    public static void main(String[] args) throws InterruptedException, IOException, URISyntaxException {
        BufferedReader c = new BufferedReader(new InputStreamReader(System.in));
        printer.Value("Press ? to get instructions.").toString();
        key = c.readLine().charAt(0);
        while (true) {
            printer.Value("Press c to get categories").toString();
            printer.Value("Press r to get random jokes").toString();
            key = c.readLine().charAt(0);
            if (key == 'c')
            {
                getCategories();
                PrintResults();
            }
            if (key == 'r')
            {
                printer.Value("Want to specify a category? y/n").toString();
                if (key == 'y')
                {
                    printer.Value("How many jokes do you want? (1-9)").toString();
                    int n = Integer.parseInt(c.readLine());
                    printer.Value("Enter a category;").toString();
                    getRandomJokes(c.readLine(), n);
                    PrintResults();
                }
                else
                {
                    printer.Value("How many jokes do you want? (1-9)").toString();
                    int n = Integer.parseInt(c.readLine());
                    getRandomJokes(null, n);
                    PrintResults();
                }
            }
            names.clear();
        }
    }

    private static void PrintResults()
    {
        printer.Value("[" + String.join(",", results) + "]").toString();
    }

    private static void getRandomJokes(String category, int number) throws InterruptedException, IOException, URISyntaxException {
        new JsonFeed("https://us-central1-geotab-interviews.cloudfunctions.net/joke", number);
        results = JsonFeed.getRandomJokes(category);
    }

    private static void getCategories() throws InterruptedException, IOException, URISyntaxException {
        new JsonFeed("https://us-central1-geotab-interviews.cloudfunctions.net/joke_category", 0);
        results = JsonFeed.getCategories();
    }
}