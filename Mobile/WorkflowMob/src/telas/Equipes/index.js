import React, { useState } from 'react';
import { StyleSheet, Text, TextInput, View, TouchableOpacity, Image, CheckBox, ScrollView} from 'react-native';

export default function Equipes({navigation}){
    return(
      <ScrollView>
        <View style={styles.container}>
            <Text style={styles.subtitulo}>Equipes</Text>
            <View style={styles.areapesquisa}>
              <TextInput
                style={styles.input}
                placeholder='Pesquisa uma pesquisa'
              ></TextInput>
            </View>
            <TouchableOpacity
              style={styles}
              onPress={()=> navigation.navigate('TarefasPedentes')}
            >
              <Image style={styles.imagemequipe}></Image>
              <Text style={styles.tituloequipe}>Equipe 0</Text>
              <Text style={styles.subtituloequipe}>Loremisadjssxckjb</Text>
            </TouchableOpacity>
        </View>
      </ScrollView>

    )
}


const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  }
})