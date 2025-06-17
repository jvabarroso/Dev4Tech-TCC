import React, { useState } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView, TextInput} from 'react-native';
import { styles } from './style';

import { Ionicons } from '@expo/vector-icons';

export default function CadastroEquipes({navigation}){

    return(
        <ScrollView style={styles.scroll}>
            <View style={styles.container}>
                <Text style={styles.titulo}>Criar uma equipe</Text>
                <View style={styles.areaInput}>
                    <Text style={styles.texto}>Nome da Equipe</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="Digite o nome da equipe"
                        placeholderTextColor={"#000000"}
                    />
                    <Text style={styles.texto}>Categoria da equipe</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="Digite a categoria da equipe"
                        placeholderTextColor={"#000000"}
                        secureTextEntry={true}
                    />
                    <Text style={styles.texto}>Adicionar membros à equipe</Text>
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
