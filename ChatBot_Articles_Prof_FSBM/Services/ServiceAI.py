# -*- coding: utf-8 -*-
from together import Together
import sys
import traceback

def Ask_Llama3_With_TogetherAI(prompt):
    client = Together(api_key = '5e4309c119d07b71ec696104dd005ff457cde97c91e5ea31fda07096b3723cc1')

    response = client.chat.completions.create(
        model="meta-llama/Llama-3.3-70B-Instruct-Turbo-Free",
        messages=[
          {
            "role": "user",
            "content": prompt
          }
        ]
    )

    return response.choices[0].message.content

if __name__ == "__main__":
    # Ton code ici
    #prompt = "What are some fun things to do in New York?"
    #response = Ask_Llama3_With_TogetherAI(prompt)
    #print(response)

    try:
        prompt = sys.argv[1]
        #print("Prompt reçu :", prompt)
        response = Ask_Llama3_With_TogetherAI(prompt)
        print(response)
    except Exception as e:
        print("Erreur dans le script Python :", str(e))
        traceback.print_exc()