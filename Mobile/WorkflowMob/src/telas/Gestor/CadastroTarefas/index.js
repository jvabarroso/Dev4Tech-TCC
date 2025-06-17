import React, { useState } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView} from 'react-native';
import { Card, Title, Paragraph } from 'react-native-paper';
import { styles } from './style';

export default function CadastroTarefas({navigation}){

    return(
        <ScrollView style={styles.scroll}>
            <View style={styles.container}>
                <Text style={styles.titulo}>Adicionar uma tarefa</Text>
                <View style={styles.areaInput}>
                    <Text style={styles.texto}>Instruções</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="Mudar"
                        placeholderTextColor={"#000000"}
                    />
                    <TouchableOpacity>
                        <Text>Mudar</Text>
                    </TouchableOpacity>
                    <Text style={styles.texto}>Equipes</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="Digite a categoria da equipe"
                        placeholderTextColor={"#000000"}
                        secureTextEntry={true}
                    />
                    <Text style={styles.texto}>Data e entrega</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="Mudar ainda"
                        placeholderTextColor={"#000000"}
                        secureTextEntry={true}
                    />
                    ainda adicionar mais coisas
                </View>
            </View>
        </ScrollView>
    )
}
