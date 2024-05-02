package com.jokecompany;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonParser;

import java.io.IOException;
import java.lang.reflect.Array;
import java.net.URI;
import java.net.URISyntaxException;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;

public class JsonFeed {
    static String url = "";

    public JsonFeed(String endpoint, int results) {
        url = endpoint;
    }

    public static String[] getRandomJokes(String category) throws URISyntaxException, IOException, InterruptedException {
        HttpClient client = HttpClient.newHttpClient();
        if (category != null)
        {
            url += "?";
            url += "category=";
            url += category;
        }
        URI uri = new URI(url);
        HttpRequest request = HttpRequest.newBuilder().uri(uri).build();
        String joke = client.send(request, HttpResponse.BodyHandlers.ofString()).body();
        var jsonObject = new JsonParser().parse(joke).getAsJsonObject();
        return new String[] {jsonObject.get("value").getAsString()};
    }

    public static String[] getCategories() throws IOException, InterruptedException, URISyntaxException {
        HttpClient client = HttpClient.newHttpClient();
        URI uri = new URI(url);
        HttpRequest request = HttpRequest.newBuilder().uri(uri).build();
        String responsebody = client.send(request, HttpResponse.BodyHandlers.ofString()).body();
        return new String[] {responsebody}; 
    }
}
