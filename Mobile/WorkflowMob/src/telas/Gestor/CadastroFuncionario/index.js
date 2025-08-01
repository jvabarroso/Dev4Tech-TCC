import React, { useState } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView, TextInput} from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'


import { Ionicons } from '@expo/vector-icons';

export default function CadastroFuncionario({navigation}){
    const { theme } = useTheme();
    const styles = getStyles(theme);
    
    const [nome, setNome] = useState('');
    const [email, setEmail] = useState('');
    const [dataNascimento, setDataNascimento] = useState('');
    const [categoria, setCategoria] = useState('');
    const [telefone, setTelefone] = useState('');
    const [endereco, setEndereco] = useState('');

    const [equipes, setequipes] = useState(true);
    const [equipeselecionada, setEquipeselecionada] = useState(null);

    const cliqueinformacao = () => 
        {
            setequipes(valorAtual => !valorAtual); 
        };
    
    const confirmodeequipe = (equipeSelecionada) => 
        {   
            setEquipeselecionada(equipeSelecionada.titulo);
            setequipes(valorAtual => !valorAtual); 
        };

    //Máscara input
    // Adicione estas funções utilitárias no topo do arquivo
    function formatarDataParaBanco(data) {
      if (!data) return '';
      
      // Se já está no formato do banco, retorna direto
      if (/^\d{4}-\d{2}-\d{2}$/.test(data)) return data;
      
      // Converte de DD/MM/AAAA para AAAA-MM-DD
      const partes = data.split('/');
      if (partes.length === 3) {
        return `${partes[2]}-${partes[1]}-${partes[0]}`;
      }
      return data;
    }

    function formatarTelefone(telefone) {
      if (!telefone) return '';
      // Remove todos os caracteres não numéricos
      return telefone.replace(/\D/g, '');
    }

    function formatarDataInput(text) {
      let data = text.replace(/\D/g, '');
      
      if (data.length > 2) data = `${data.slice(0,2)}/${data.slice(2)}`;
      if (data.length > 5) data = `${data.slice(0,5)}/${data.slice(5,9)}`;
      
      return data.slice(0,10);
    }

    function formatarTelefoneInput(text) {
      let tel = text.replace(/\D/g, '');
      
      if (tel.length > 0) tel = `(${tel}`;
      if (tel.length > 3) tel = `${tel.slice(0,3)}) ${tel.slice(3)}`;
      if (tel.length > 10) tel = `${tel.slice(0,10)}-${tel.slice(10,15)}`;
      
      return tel.slice(0,15);
    }


    return(
   <View style={styles.container}>
        <ScrollView contentContainerStyle={styles.scrollContent}>
            <Text style={styles.titulo}>Cadastrar Funcionário</Text>
                    <View style={styles.areaInput}>
                        <Text style={styles.texto}>Nome do funcionário</Text>
                        <TextInput
                            style={styles.input}
                            value={nome}
                            placeholder="Gabriel Kenzo" //depois mudar, mensagem para mim mesmo dnv :D
                            placeholderTextColor={theme.text}
                        />
                        <Text style={styles.texto}>Data de nascimento</Text>
                        <TextInput
                            style={styles.input}
                            value={dataNascimento}
                            placeholder="xx/xx/xxxx"
                            placeholderTextColor={theme.text}
                            onChangeText={(text) => setDataNascimento(formatarDataInput(text))}
                        />
                        <Text style={styles.texto}>Email</Text>
                        <TextInput
                            style={styles.input}
                            value={email}
                            placeholder="joaovitinhocraft@gmail.com"
                            placeholderTextColor={theme.text}
                        />
                        <Text style={styles.texto}>Telefone</Text>
                        <TextInput
                            style={styles.input}
                            value={telefone}
                            placeholder="1399899989"
                            placeholderTextColor={theme.text}
                            onChangeText={(text) => setTelefone(formatarTelefoneInput(text))}
                        />
                        <Text style={styles.texto}>Endereço</Text>
                        <TextInput
                            style={styles.input}
                            value={endereco}
                            placeholder="Rua João da Fonseca, Jardim Mato Grosso, Cananeia senha"
                            placeholderTextColor={theme.text}
                        />
                        <Text style={styles.texto}>Categoria do funcionário</Text>
                        <TextInput
                            style={styles.input}
                            value={categoria}
                            placeholder="Analista"
                            placeholderTextColor={theme.text}
                        />
                        <Text style={styles.texto}>Adicionar a uma equipe</Text>
                        <TouchableOpacity
                            style={[styles.input, styles.linha]}
                            onPress={cliqueinformacao}
                        >
                            <Text style={styles.textobotao}>{equipeselecionada || "Selecione uma equipe"}</Text> 
                            <Ionicons name="caret-down-outline" size={18} color="black" />
                        </TouchableOpacity>
                    </View>
                {!equipes && equipe.map(item => (
                    <TouchableOpacity
                        key={item.id}
                        style={styles.containerequipes}
                        onPress={() => confirmodeequipe(item)}
                    >
                    <Image source={item.imagem} style={styles.imag} />
                    <View style={styles.textos}>
                        <Text style={styles.textolistatitulo}>{item.titulo}</Text>
                        <Text style={styles.textolistacargo}>{item.cargo}</Text>
                    </View>
                    </TouchableOpacity>
                ))}
                <TouchableOpacity style={styles.botaocriar}>
                    <Text style={styles.textocriar}>Criar</Text>
                </TouchableOpacity>
            </ScrollView>
        </View>
    );
}