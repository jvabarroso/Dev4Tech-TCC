import React, { useState } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView, TextInput} from 'react-native';
import { styles } from './style';

export default function CadastroFuncionario({navigation}){

    return(
        <ScrollView style={styles.scroll}>
            <View style={styles.container}>
                <Text style={styles.titulo}>Cadastrar Funcionário</Text>
                <View style={styles.areaInput}>
                    <Text style={styles.texto}>Nome</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="Gabriel Kenzo" //depois mudar, mensagem para mim mesmo dnv :D
                        placeholderTextColor={"#000000"}
                    />
                    <Text style={styles.texto}>Data de nascimento</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="xx/xx/xxxx"
                        placeholderTextColor={"#000000"}
                        secureTextEntry={true}
                    />
                    <Text style={styles.texto}>Email</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="joaovitinhocraft@gmail.com"
                        placeholderTextColor={"#000000"}
                        secureTextEntry={true}
                    />
                    <Text style={styles.texto}>Telefone</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="1399899989"
                        placeholderTextColor={"#000000"}
                    />
                    <Text style={styles.texto}>Endereço</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="Rua João da Fonseca, Jardim Mato Grosso, Cananeia senha"
                        placeholderTextColor={"#000000"}
                        secureTextEntry={true}
                    />
                    <Text style={styles.texto}>Categoria do funcionario</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="Analista de Marketing"
                        placeholderTextColor={"#000000"}
                        secureTextEntry={true}
                    />
                </View>
            </View>
        </ScrollView>
    )
}
